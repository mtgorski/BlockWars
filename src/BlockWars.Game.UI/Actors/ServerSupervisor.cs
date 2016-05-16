using Akka.Actor;
using System;
using BlockWars.Game.UI.Commands;
using BlockWars.GameState.Client;
using BlockWars.GameState.Models;
using System.Collections.Generic;
using Akka.DI.Core;
using BlockWars.Game.UI.Strategies;

namespace BlockWars.Game.UI.Actors
{
    public class ServerSupervisor : ReceiveActor
    { 
        private readonly IGameStateClient _gameClient;
        private readonly INewRegionsFactory _regionsStrategy;
        private readonly INewLeagueFactory _leagueStrategy;

        public ServerSupervisor(
            IGameStateClient gameClient,
            INewLeagueFactory newLeagueStrategy,
            INewRegionsFactory newRegionsStrategy)
        {
            _gameClient = gameClient;
            _leagueStrategy = newLeagueStrategy;
            _regionsStrategy = newRegionsStrategy;

            InitializeLeague(true);

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
                InitializeLeague(false);
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

        private void InitializeLeague(bool initializingServer)
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

        }

    }
}
