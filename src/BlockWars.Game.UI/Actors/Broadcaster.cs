using Akka.Actor;
using BlockWars.Game.UI.ViewModels;
using Microsoft.AspNet.SignalR.Infrastructure;

namespace BlockWars.Game.UI
{
    public class Broadcaster : ReceiveActor
    {
        private readonly IConnectionManager _connectionManager;

        public Broadcaster(IConnectionManager connectionManager)
        {
            _connectionManager = connectionManager;

            Receive<LeagueViewModel>(x =>
            {
                Broadcast(x);
                return true;
            });
        }

        private void Broadcast(LeagueViewModel currentLeague)
        {
            var hub = _connectionManager.GetHubContext<GameHub>();
            hub.Clients.All.updateRegionInfo(currentLeague);
        }
    }
}