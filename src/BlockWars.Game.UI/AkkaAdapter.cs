using System;
using Akka.Actor;
using BlockWars.Game.UI.Commands;
using BlockWars.Game.UI.Models;
using Akka.DI.Core;
using BlockWars.Game.UI.Actors;

namespace BlockWars.Game.UI
{
    /// <summary>
    /// A wrapper to the Akka actor system to allow clients to interact with the system
    /// without know Akka details
    /// </summary>
    public class AkkaAdapter : IServerManager
    {
        private readonly ActorSystem _actorSystem;

        public AkkaAdapter(ActorSystem actorSystem)
        {
            _actorSystem = actorSystem; 
        }

        public void AddRegion(Guid leagueId, RegionState region)
        {
            _actorSystem.ActorSelection("/user/supervisor/" + leagueId.ToString()).Tell(new AddRegionCommand(leagueId, region));
        }

        public void AddStatsActor(string connectionId)
        {
            var statsActor = _actorSystem.ActorOf(_actorSystem.DI().Props<UserStatsActor>(), "stats" + connectionId);
            _actorSystem.EventStream.Subscribe(statsActor, typeof(LeagueEndedMessage));
        }

        public void BuildBlock(Guid leagueId, string regionName, string connectionId)
        {
            var command = new BuildBlockCommand(leagueId, regionName, connectionId);
            _actorSystem.ActorSelection("/user/supervisor/" + leagueId.ToString()).Tell(command);
        }

        public void RemoveStatsActor(string connectionId)
        {
            _actorSystem.ActorSelection("/user/stats" + connectionId).Tell(PoisonPill.Instance);
        }
    }
}
