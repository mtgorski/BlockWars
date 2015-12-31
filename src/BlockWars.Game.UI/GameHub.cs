using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using BlockWars.GameState.Client;
using System.Net.Http;
using System;
using System.Threading;

namespace BlockWars.Game.UI
{
    [HubName("game")]
    public class GameHub : Hub
    {
        private static Timer _timer;
        private static readonly object _lock = new object();

        public override async Task OnConnected()
        {
            var client = new GameStateClient(new HttpClient(), "http://localhost:5000");
            var regions = await client.GetRegionsAsync(Guid.Parse("5a44f339-3133-4f79-a6dc-1862568569cc"));
            Clients.Caller.updateRegionInfo(regions);

            EnsureTimer();
        }

        private void EnsureTimer()
        {
            lock(_lock)
            {
                if (_timer == null)
                {
                    _timer = new Timer(async _ => await NotifyClients(Clients), null, 0, 500);
                }
            }
        }

        private static async Task NotifyClients(IHubCallerConnectionContext<dynamic> clients)
        {
            var httpClient = new GameStateClient(new HttpClient(), "http://localhost:5000");
            var regions = await httpClient.GetRegionsAsync(Guid.Parse("5a44f339-3133-4f79-a6dc-1862568569cc"));
            clients.All.updateRegionInfo(regions);
        }

    }
}
