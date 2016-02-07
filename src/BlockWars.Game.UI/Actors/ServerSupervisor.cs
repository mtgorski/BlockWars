using Akka.Actor;
using System;
using BlockWars.Game.UI.Commands;
using System.Threading.Tasks;
using BlockWars.GameState.Client;
using BlockWars.GameState.Models;
using System.Collections.Generic;
using Akka.DI.Core;

namespace BlockWars.Game.UI.Actors
{
    public class ServerSupervisor : ReceiveActor
    { 
        private readonly IGameStateClient _gameClient;
        private readonly INewRegionsStrategy _regionsStrategy;
        private readonly INewLeagueStrategy _leagueStrategy;

        public ServerSupervisor(
            IGameStateClient gameClient,
            INewLeagueStrategy newLeagueStrategy,
            INewRegionsStrategy newRegionsStrategy)
        {
            _gameClient = gameClient;
            _leagueStrategy = newLeagueStrategy;
            _regionsStrategy = newRegionsStrategy;

            InitializeLeagueAsync(true).GetAwaiter().GetResult();

            Context.System.Scheduler.ScheduleTellRepeatedly(
                TimeSpan.FromSeconds(0),
                TimeSpan.FromMilliseconds(15),
                Context.Self,
                new PingLeaguesCommand(),
                Context.Self);


            Receive<PingLeaguesCommand>(x =>
            {
                PingLeagues(x);
                return true;
            });

            Receive<LeagueEndedMessage>(x =>
            {
                InitializeLeagueAsync(false).GetAwaiter().GetResult();
                return true;
            });
        }

        private void PingLeagues(PingLeaguesCommand x)
        {
            var children = Context.GetChildren();
            foreach(var child in children)
            {
                child.Tell(new CheckStateCommand());
            }
        }

        private Task InitializeLeagueAsync(bool initializingServer)
        {
            var league = initializingServer ? _gameClient.GetCurrentLeagueAsync().GetAwaiter().GetResult() : null;
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

            var currentLeague = Context.ActorOf(Context.System.DI().Props<LeagueActor>(), league.LeagueId.ToString());
            currentLeague.Tell(new InitializeLeagueCommand(league));
            foreach (var region in regions)
            {
                currentLeague.Tell(new AddRegionCommand(league.LeagueId, region));
            }

            return Task.FromResult(0);
        }

    }
}
