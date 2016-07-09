using System;
using Akka.Actor;
using BlockWars.Game.UI.ViewModels;
using System.Linq;
using BlockWars.Game.UI.Commands;
using Microsoft.Extensions.OptionsModel;
using BlockWars.Game.UI.Options;

namespace BlockWars.Game.UI.Actors
{
    public class DemoActor : ReceiveActor
    {
        private static Random Rng = new Random();
        private readonly IServerManager _serverManager;
        private LeagueViewModel? _currentLeague;
        private readonly IOptions<DemoOptions> _options;

        public DemoActor(IServerManager serverManager, IOptions<DemoOptions> options)
        {
            _serverManager = serverManager;
            _options = options;

            Receive<LeagueViewModel>(x =>
            {
                _currentLeague = x;
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
            if(_currentLeague == null)
            {
                return;
            }
            var whichRegionIndex = Rng.Next(_currentLeague.Value.Regions.Count);
            var whichRegion = _currentLeague.Value.Regions.Where((_, i) => i == whichRegionIndex).Single();

            for(int i = 0; i < _options.Value.DemoBlocksPerCommand; i++)
            {
                _serverManager.BuildBlock(_currentLeague.Value.League.LeagueId, whichRegion.Name, "");
            }            
        }
    }
}
