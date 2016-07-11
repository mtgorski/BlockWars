using Akka.Actor;
using Microsoft.AspNet.SignalR.Infrastructure;
using System;
using System.Collections.Generic;

namespace BlockWars.Game.UI.Actors
{
    public class PlayerStatsActor : ReceiveActor
    {
        private readonly IConnectionManager _connectionManager;
        private Dictionary<Guid, int> _leagueToCountMap = new Dictionary<Guid, int>();
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

            Receive<LeagueEndedMessage>(x =>
            {
                OnLeagueEnd(x);
                return true;
            });
        }

        private void OnLeagueEnd(LeagueEndedMessage x)
        {
            _leagueToCountMap.Remove(x.LeagueId);
        }

        private void OnBlockBuilt(BlockBuiltMessage x)
        {
            _connectionId = x.ConnectionId;

            if(!_leagueToCountMap.ContainsKey(x.LeagueId))
            {
                _leagueToCountMap[x.LeagueId] = 0;
            }

            var blockCount = ++_leagueToCountMap[x.LeagueId];

            var accomplishment = _accomplishmentManager.GetAccomplishment(blockCount);
            if(accomplishment != null)
            {
                var hub = _connectionManager.GetHubContext<GameHub>();
                hub.Clients.Client(x.ConnectionId).onAchieve(accomplishment);
            }
        }

    }
}
