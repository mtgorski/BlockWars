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
using Akka.Event;

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
        private bool _gameLoopEnsured;
        private readonly object _lock = new object();
        private IActorRef _loopPinger;

        public AkkaAdapter(IServiceProvider serviceProvider)
        {
            _actorSystem = ActorSystem.Create("BlockWars");
            _actorSystem.AddDependencyResolver(new ActorContainer(serviceProvider, _actorSystem));
            _serverSupervisor = _actorSystem.ActorOf(_actorSystem.DI().Props<ServerSupervisor>());
            _loopPinger = _actorSystem.ActorOf(_actorSystem.DI().Props<LoopPinger>());
        }

        public CancellationTokenSource GameLoopCancellationSource
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public void AddRegion(Guid leagueId, Region region)
        {
            var command = new AddRegionCommand(leagueId, region);
            _serverSupervisor.Tell(command);
        }

        public void BuildBlock(Guid leagueId, string regionName)
        {
            var command = new BuildBlockCommand(leagueId, regionName);
            _serverSupervisor.Tell(command);
        }

        public void EnsureGameLoop(IHubCallerConnectionContext<dynamic> clients)
        {
           lock(_lock)
            {
                if(!_gameLoopEnsured)
                {
                    _actorSystem.Scheduler.ScheduleTellRepeatedly(
                        TimeSpan.Zero,
                        TimeSpan.FromMilliseconds(14),
                        _serverSupervisor,
                        new RunGameLoopCommand(clients),
                        _loopPinger);
                      
                    _gameLoopEnsured = true;
                }
            }
        }

        public LeagueViewModel GetCurrentLeagueView()
        {
            var query = new CurrentLeagueViewQuery();
            return _serverSupervisor.Ask<LeagueViewModel>(query).GetAwaiter().GetResult();
        }
    }
}
