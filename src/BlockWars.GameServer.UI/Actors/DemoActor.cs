using System;
using Akka.Actor;
using BlockWars.Game.UI.ViewModels;
using System.Linq;
using BlockWars.Game.UI.Commands;
using Microsoft.Extensions.Configuration;
using BlockWars.Game.UI.Options;
using Microsoft.Extensions.Options;

namespace BlockWars.Game.UI.Actors
{
    public class DemoActor : ReceiveActor
    {
        private static Random Rng = new Random();
        private readonly IServerManager _serverManager;
        private GameViewModel? _currentGame;
        private readonly DemoOptions _options;

        public DemoActor(IServerManager serverManager, IOptions<DemoOptions> options)
        {
            _serverManager = serverManager;
            _options = options.Value;

            Receive<GameViewModel>(x =>
            {
                _currentGame = x;
                return true;
            });

            Receive<SendDemoClickCommand>(x =>
            {
                SendDemoClick(x);
                return true;
            });

            Context.System.Scheduler.ScheduleTellRepeatedly(
                TimeSpan.FromSeconds(0),
                TimeSpan.FromMilliseconds(50),
                Self,
                new SendDemoClickCommand(),
                Self);

        }

        private void SendDemoClick(SendDemoClickCommand command)
        {
            if(_currentGame == null)
            {
                return;
            }
            var whichRegionIndex = Rng.Next(_currentGame.Value.Regions.Count);
            var whichRegion = _currentGame.Value.Regions.Where((_, i) => i == whichRegionIndex).Single();

            for(int i = 0; i < _options.DemoBlocksPerCommand; i++)
            {
                Context.ActorSelection("/user/supervisor/" + _currentGame.Value.Game.GameId).Tell(new BuildBlockCommand(_currentGame.Value.Game.GameId, whichRegion.Name, "DemoBot"));
            }            
        }
    }
}
