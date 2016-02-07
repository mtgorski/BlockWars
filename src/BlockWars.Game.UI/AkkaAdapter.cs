using System;
using System.Threading;
using BlockWars.Game.UI.ViewModels;
using BlockWars.GameState.Models;
using Microsoft.AspNet.SignalR.Hubs;
using Akka.Actor;
using BlockWars.Game.UI.Actors;
using Akka.DI.Core;
using BlockWars.Game.UI.IoC;
using BlockWars.Game.UI.Commands;

namespace BlockWars.Game.UI
{
    /// <summary>
    /// A wrapper to the Akka actor system in order to provide consistency
    /// with the other implementation
    /// </summary>
    public class AkkaAdapter : IServerManager
    {
        private readonly IActorRef _serverSupervisor;
        private readonly ActorSystem _actorSystem;
        private readonly object _lock = new object();
        private readonly IActorRef _broadcaster;


        public AkkaAdapter(IServiceProvider serviceProvider)

        {
            _actorSystem = ActorSystem.Create("BlockWars");
            _actorSystem.AddDependencyResolver(new ActorContainer(serviceProvider, _actorSystem));
            _serverSupervisor = _actorSystem.ActorOf(_actorSystem.DI().Props<ServerSupervisor>(), "supervisor");
            _broadcaster = _actorSystem.ActorOf(_actorSystem.DI().Props<Broadcaster>(), "broadcaster");
            _actorSystem.ActorOf(_actorSystem.DI().Props<DemoActor>(), "demo");
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
