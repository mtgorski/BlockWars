using System;
using BlockWars.GameState.Models;
using Akka.Actor;
using BlockWars.Game.UI.Commands;

namespace BlockWars.Game.UI
{
    /// <summary>
    /// A wrapper to the Akka actor system in order to provide consistency
    /// with the other implementation
    /// </summary>
    public class AkkaAdapter : IServerManager
    {
        private readonly ActorSystem _actorSystem;

        public AkkaAdapter(ActorSystem actorSystem)
        {
            _actorSystem = actorSystem; 
        }

        public void AddRegion(Guid leagueId, Region region)
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
