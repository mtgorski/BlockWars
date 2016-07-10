using System;
using Akka.Actor;
using BlockWars.Game.UI.Commands;
using BlockWars.Game.UI.Models;
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

        public void AddConnectedUser(string connectionId)
        {
            _actorSystem.EventStream.Publish(new UserConnectedMessage(connectionId));
        }

        public void BuildBlock(Guid leagueId, string regionName, string connectionId)
        {
            var command = new BuildBlockCommand(leagueId, regionName, connectionId);
            _actorSystem.ActorSelection("/user/supervisor/" + leagueId.ToString()).Tell(command);
        }

        public void RemoveConnectedUser(string connectionId)
        {
            _actorSystem.EventStream.Publish(new UserDisconnectedMessage(connectionId));
        }
    }
}
