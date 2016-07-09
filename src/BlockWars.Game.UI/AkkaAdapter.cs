using System;
using Akka.Actor;
using BlockWars.Game.UI.Commands;
using BlockWars.Game.UI.Models;

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

        public void BuildBlock(Guid leagueId, string regionName)
        {
            var command = new BuildBlockCommand(leagueId, regionName);
            _actorSystem.ActorSelection("/user/supervisor/" + leagueId.ToString()).Tell(command);
        }
    }
}
