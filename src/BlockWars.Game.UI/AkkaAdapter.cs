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

        public void AddRegion(Guid gameId, RegionState region, string connectionId)
        {
            _actorSystem.ActorSelection("/user/playerSupervisor/player" + connectionId).Tell(new AddRegionCommand(gameId, region));
        }

        public void AddConnectedUser(string connectionId)
        {
            _actorSystem.EventStream.Publish(new UserConnectedMessage(connectionId));
        }

        public void BuildBlock(Guid gameId, string regionName, string connectionId)
        {
            var command = new BuildBlockCommand(gameId, regionName, connectionId);
            _actorSystem.ActorSelection("/user/playerSupervisor/player" + connectionId).Tell(command);
        }

        public void RemoveConnectedUser(string connectionId)
        {
            _actorSystem.EventStream.Publish(new UserDisconnectedMessage(connectionId));
        }
    }
}
