using Akka.Actor;
using System;
using BlockWars.Game.UI.Commands;
using BlockWars.GameState.Client;
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

            InitializeLeague();

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
                Context.Sender.Tell(PoisonPill.Instance);
                InitializeLeague();
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

        private void InitializeLeague()
        {
            var league = _leagueStrategy.GetLeague();
            var regions = _regionsStrategy.GetRegions();

            var currentLeague = Context.ActorOf(Context.System.DI().Props<LeagueActor>(), league.LeagueId.ToString());
            currentLeague.Tell(new InitializeLeagueCommand(league, regions));
        }

    }
}
