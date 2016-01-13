using Akka.Actor;
using System;
using BlockWars.Game.UI.ViewModels;
using BlockWars.Game.UI.Commands;
using System.Threading.Tasks;
using BlockWars.GameState.Client;
using BlockWars.GameState.Models;
using System.Collections.Generic;
using Microsoft.AspNet.SignalR.Hubs;
using Akka.DI.Core;
using System.Diagnostics;

namespace BlockWars.Game.UI.Actors
{
    public class ServerSupervisor : ReceiveActor
    {
        private Guid _currentLeagueId;
        private DateTime _currentLeagueExpirationTime;
        private readonly IGameStateClient _gameClient;
        private readonly INewRegionsStrategy _regionsStrategy;
        private readonly INewLeagueStrategy _leagueStrategy;
        private bool _initialized;
        private IHubCallerConnectionContext<dynamic> _clients;

        public ServerSupervisor(
            IGameStateClient gameClient,
            INewLeagueStrategy newLeagueStrategy,
            INewRegionsStrategy newRegionsStrategy)
        {
            _gameClient = gameClient;
            _leagueStrategy = newLeagueStrategy;
            _regionsStrategy = newRegionsStrategy;

            Receive<AddRegionCommand>(x =>
            {
                AddRegion(x);
                return true;
            });
            Receive<BuildBlockCommand>(x =>
            {
                BuildBlock(x);
                return true;
            });
            
            Receive<CurrentLeagueViewQuery>(x =>
            {
                var view = GetCurrentLeagueView();
                Sender.Tell(view);
                return true;
            });

            Receive<RunGameLoopCommand>(x =>
            {
                Debug.WriteLine("running");
                GameLoopAsync(x.Clients).Wait();
                Debug.WriteLine("end run");
                return true;
            });
        }

        private Task InitializeLeagueAsync(IHubCallerConnectionContext<dynamic> clients)
        {
            _clients = clients;

            var league = _gameClient.GetCurrentLeagueAsync().GetAwaiter().GetResult();
            ICollection<Region> regions;
            if (league == null)
            {
                league = _leagueStrategy.GetLeague();
                regions = _regionsStrategy.GetRegions();
            }
            else
            {
                regions = _gameClient.GetRegionsAsync(league.LeagueId).GetAwaiter().GetResult();
            }

            _currentLeagueId = league.LeagueId;
            _currentLeagueExpirationTime = league.ExpiresAt;
            var currentLeague = Context.ActorOf<LeagueActor>(league.LeagueId.ToString());
            foreach (var region in regions)
            {
                currentLeague.Tell(new AddRegionCommand(league.LeagueId, region));
            }

            _initialized = true;

            return Task.FromResult(0);
        }

        private Task GameLoopAsync(IHubCallerConnectionContext<dynamic> clients)
        { 
            if(!_initialized)
            {
                InitializeLeagueAsync(clients).GetAwaiter().GetResult();
            }   

            var currentLeague = Context.Child(_currentLeagueId.ToString());

            if (_currentLeagueExpirationTime < DateTime.UtcNow)
            {
                currentLeague.Tell(new EndLeagueCommand());
                var league = _leagueStrategy.GetLeague();
                var regions = _regionsStrategy.GetRegions();
                currentLeague = Context.ActorOf(Context.System.DI().Props<LeagueActor>(), league.LeagueId.ToString());
                foreach (var region in regions)
                {
                    currentLeague.Tell(new AddRegionCommand(league.LeagueId, region));
                }
                _currentLeagueId = league.LeagueId;
                _currentLeagueExpirationTime = league.ExpiresAt;
            }
            else
            {
                var view = GetCurrentLeagueView();
                _clients.All.updateRegionInfo(view);
            }

            return Task.FromResult(0);
        }

        private void BuildBlock(BuildBlockCommand x)
        {
            if(x.LeagueId == _currentLeagueId)
            {
                var leagueActor = Context.Child(x.LeagueId.ToString());
                leagueActor.Tell(x);
            }
        }

        private void AddRegion(AddRegionCommand x)
        {
            if(x.LeagueId == _currentLeagueId)
            {
                var leagueActor = Context.Child(x.LeagueId.ToString());
                leagueActor.Tell(x);
            }
        }

        private LeagueViewModel GetCurrentLeagueView()
        {
            var leagueActor = Context.Child(_currentLeagueId.ToString());
            var leagueView = leagueActor.Ask<LeagueViewModel>(new GetViewQuery()).GetAwaiter().GetResult();
            leagueView.League = new League
            {
                ExpiresAt = _currentLeagueExpirationTime,
                LeagueId = _currentLeagueId
            };
            return leagueView;
        }
    }
}
