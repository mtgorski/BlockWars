using Akka.Actor;
using BlockWars.Game.UI.ViewModels;
using Microsoft.AspNet.SignalR.Infrastructure;

namespace BlockWars.Game.UI
{
    public class Broadcaster : ReceiveActor
    {
        private LeagueViewModel _lastView;

        public Broadcaster(IConnectionManager connectionManager)
        {
            Receive<LeagueViewModel>(x =>
            {
                _lastView = x;
                var hub = connectionManager.GetHubContext<GameHub>();
                hub.Clients.All.updateRegionInfo(x);
                return true;
            });

            Receive<CurrentLeagueViewQuery>(x =>
            {
                Sender.Tell(_lastView);
                return true;
            });
        }
    }
}