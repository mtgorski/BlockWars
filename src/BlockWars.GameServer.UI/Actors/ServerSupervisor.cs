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
        private readonly INewGameFactory _gameStrategy;

        public ServerSupervisor(
            IGameStateClient gameClient,
            INewGameFactory newGameStrategy,
            INewRegionsFactory newRegionsStrategy)
        {
            _gameClient = gameClient;
            _gameStrategy = newGameStrategy;
            _regionsStrategy = newRegionsStrategy;

            InitializeGames();

            Context.System.Scheduler.ScheduleTellRepeatedly(
                TimeSpan.FromSeconds(0),
                TimeSpan.FromMilliseconds(15),
                Context.Self,
                new PingGamesCommand(),
                Context.Self);


            Receive<PingGamesCommand>(x =>
            {
                PingGames(x);
                return true;
            });

            Receive<GameEndedMessage>(x =>
            {
                Context.Sender.Tell(PoisonPill.Instance);
                InitializeGames();
                return true;
            });
        }

        private void PingGames(PingGamesCommand x)
        {
            var children = Context.GetChildren();
            foreach(var child in children)
            {
                child.Tell(new CheckStateCommand());
            }
        }

        private void InitializeGames()
        {
            var game = _gameStrategy.GetGameState();
            var regions = _regionsStrategy.GetRegions();

            var currentGame = Context.ActorOf(Context.System.DI().Props<GameActor>(), game.GameId.ToString());
            currentGame.Tell(new InitializeGameCommand(game, regions));
        }

    }
}
