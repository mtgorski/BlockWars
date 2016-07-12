using Akka.Actor;
using Microsoft.AspNet.SignalR.Infrastructure;
using System;
using System.Collections.Generic;

namespace BlockWars.Game.UI.Actors
{
    public class PlayerStatsActor : ReceiveActor
    {
        private readonly IConnectionManager _connectionManager;
        private Dictionary<Guid, int> _gameToCountMap = new Dictionary<Guid, int>();
        private AccomplishmentManager _accomplishmentManager;
        private string _connectionId;

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
        }

        private void OnGameEnd(GameEndedMessage x)
        {
            _gameToCountMap.Remove(x.GameId);
        }

        private void OnBlockBuilt(BlockBuiltMessage x)
        {
            _connectionId = x.ConnectionId;

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
