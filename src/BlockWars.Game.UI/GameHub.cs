using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using BlockWars.GameState.Client;
using System.Net.Http;
using System;
using System.Threading;
using BlockWars.GameState.Models;
using System.Collections.Generic;

namespace BlockWars.Game.UI
{
    [HubName("game")]
    public class GameHub : Hub
    {
        private static GameManager _currentGameManager = GetManager();
        private static IHubCallerConnectionContext<dynamic> _clients;
        private static readonly object _lock = new object();
        private static Timer _timer;

        public override Task OnConnected()
        {
            lock(_lock)
            {
                if(_clients == null)
                {
                    _clients = Clients;
                    NotifyLoop();
                }
            }
            return Task.FromResult(0);
        }

        public void BuildBlock(string regionName)
        {
            _currentGameManager.BuildBlock(regionName);
        }

        private static GameManager GetManager()
        {
            var httpClient = new GameStateClient(new HttpClient(), "http://localhost:5000");
            var league = httpClient.GetCurrentLeagueAsync().Result;
            ICollection<Region> regions = new List<Region>();
            if (league == null)
            {
                var manager = GetManagerForNewGame();
                return manager;
            }
            else
            {
                regions = httpClient.GetRegionsAsync(league.LeagueId).Result;
            }
            var gameState = new GameState(league);
            

            return new GameManager(gameState, httpClient);
        }

        private static GameManager GetManagerForNewGame()
        {
            var httpClient = new GameStateClient(new HttpClient(), "http://localhost:5000");
            ICollection<Region> regions = new List<Region>();
            var nextExpiryTime = DateTime.UtcNow.AddMinutes(1);
            var roundedExpiryTime = new DateTime(nextExpiryTime.Year, nextExpiryTime.Month, nextExpiryTime.Day, nextExpiryTime.Hour, nextExpiryTime.Minute, 0, DateTimeKind.Utc);
            var league = new League
            {
                LeagueId = Guid.NewGuid(),
                Name = DateTime.UtcNow.ToString(),
                Description = "Automatically generated league",
                ExpiresAt = roundedExpiryTime
            };

            httpClient.PutLeagueAsync(league.LeagueId, league).Wait();

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
            regions.Add(region1);
            regions.Add(region2);
            httpClient.PutRegionAsync(league.LeagueId, region1.RegionId, region1).Wait();
            httpClient.PutRegionAsync(league.LeagueId, region2.RegionId, region2).Wait();

            var gameState = new GameState(league);
            foreach (var region in regions)
            {
                gameState.AddRegion(region);
            }

            return new GameManager(gameState, httpClient);
        }

        private static void NotifyLoop()
        {
            var viewModel = _currentGameManager.GetCurrentLeague();
            if(viewModel != null)
            {
                if (_clients != null)
                {
                    _clients.All.updateRegionInfo(viewModel);
                }
            }
            else
            {
                _currentGameManager.SaveGameAsync().GetAwaiter().GetResult();
                _currentGameManager = GetManagerForNewGame();
            }
            

            _timer = new Timer(_ => NotifyLoop(), null, 16, Timeout.Infinite);
        }

    }
}
