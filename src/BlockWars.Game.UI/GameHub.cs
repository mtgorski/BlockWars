using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using BlockWars.GameState.Client;
using System.Net.Http;
using System;
using System.Threading;
using BlockWars.GameState.Models;

namespace BlockWars.Game.UI
{
    [HubName("game")]
    public class GameHub : Hub
    {
        private static Timer _timer;
        private static readonly object _lock = new object();

        public override async Task OnConnected()
        {
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
            var currentLeague = await httpClient.GetCurrentLeagueAsync();
            if(currentLeague == null)
            {
                var nextExpiryTime = DateTime.UtcNow.AddMinutes(1);
                var roundedExpiryTime = new DateTime(nextExpiryTime.Year, nextExpiryTime.Month, nextExpiryTime.Day, nextExpiryTime.Hour, nextExpiryTime.Minute, 0, DateTimeKind.Utc);
                var newLeague = new League
                {
                    LeagueId = Guid.NewGuid(),
                    Name = DateTime.UtcNow.ToString(),
                    Description = "Automatically generated league",
                    ExpiresAt = roundedExpiryTime
                };

                await httpClient.PutLeagueAsync(newLeague.LeagueId, newLeague);
                var region1 = new Region
                {
                    Name = "Cats",
                    RegionId = Guid.NewGuid()
                };
                var region2 = new Region
                {
                    Name = "Dogs",
                    RegionId = Guid.NewGuid()
                };
                await httpClient.PutRegionAsync(newLeague.LeagueId, region1.RegionId, region1);
                await httpClient.PutRegionAsync(newLeague.LeagueId, region2.RegionId, region2);
                return;
            }

            var regions = await httpClient.GetRegionsAsync(currentLeague.LeagueId);
            if(regions.Count > 0)
            {
                clients.All.updateRegionInfo(regions);
            }       
        }

    }
}
