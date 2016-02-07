using System;
using Akka.Actor;
using BlockWars.Game.UI.ViewModels;
using System.Linq;
using BlockWars.Game.UI.Commands;
using System.Threading.Tasks;

namespace BlockWars.Game.UI.Actors
{
    public class DemoActor : ReceiveActor
    {
        private static Random Rng = new Random();
        private readonly IServerManager _serverManager;
        private LeagueViewModel _currentLeague;

        public DemoActor(IServerManager serverManager)
        {
            _serverManager = serverManager;
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
                TimeSpan.FromMilliseconds(1),
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
            var whichRegionIndex = Rng.Next(_currentLeague.Regions.Count);
            var whichRegion = _currentLeague.Regions.Where((_, i) => i == whichRegionIndex).Single();
            Parallel.For(0, 100, _ =>
            {
               _serverManager.BuildBlock(_currentLeague.League.LeagueId, whichRegion.Name);
            });
            
        }
    }
}
