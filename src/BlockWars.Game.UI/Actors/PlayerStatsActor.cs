using Akka.Actor;
using Microsoft.AspNet.SignalR.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BlockWars.Game.UI.Actors
{
    public class PlayerStatsActor : ReceiveActor
    {
        private readonly IConnectionManager _connectionManager;
        private Dictionary<Guid, int> _gameToCountMap = new Dictionary<Guid, int>();
        private AccomplishmentManager _accomplishmentManager;
        private string _connectionId;
        private bool _dirty;

        public PlayerStatsActor(IConnectionManager connectionManager, AccomplishmentManager accomplishmentManger)
        {
            _connectionManager = connectionManager;
            _accomplishmentManager = accomplishmentManger;

            Receive<BlockBuiltMessage>(x =>
            {
                OnBlockBuilt(x);
                return true;
            });

            Receive<GameEndedMessage>(x =>
            {
                OnGameEnd(x);
                return true;
            });

            Receive<BroadcastCommand>(x =>
            {
                Broadcast(x);
                return true;
            });
        }

        private void Broadcast(BroadcastCommand x)
        {
            if(_dirty)
            {
                var hub = _connectionManager.GetHubContext<GameHub>();
                var game = _gameToCountMap.FirstOrDefault();
                if (!game.Equals(default(KeyValuePair<Guid, int>)))
                    hub.Clients.Client(_connectionId).updateBlockCount(new { GameId = game.Key, Blocks = game.Value });
            }
        }

        private void OnGameEnd(GameEndedMessage x)
        {
            _gameToCountMap.Remove(x.GameId);
            _dirty = true;
        }

        private void OnBlockBuilt(BlockBuiltMessage x)
        {
            _connectionId = x.ConnectionId;
            _dirty = true;

            if(!_gameToCountMap.ContainsKey(x.GameId))
            {
                _gameToCountMap[x.GameId] = 0;
            }

            var blockCount = ++_gameToCountMap[x.GameId];

            var accomplishment = _accomplishmentManager.GetAccomplishment(blockCount);
            if(accomplishment != null)
            {
                var hub = _connectionManager.GetHubContext<GameHub>();
                hub.Clients.Client(x.ConnectionId).onAchieve(accomplishment);
            }
        }

    }
}
