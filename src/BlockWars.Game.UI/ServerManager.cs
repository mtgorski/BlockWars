
using BlockWars.GameState.Client;
using BlockWars.GameState.Models;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace BlockWars.Game.UI
{
    public class ServerManager
    {
        public static ServerManager Instance { get; } = new ServerManager();
        
        public CancellationTokenSource GameLoopCancellationSource { get; private set; }

        public GameManager CurrentGameManager
        {
            get; private set;
        }

        private readonly object _gameLoopLock = new object();
        private Task _gameLoop;
        
        private IHubCallerConnectionContext<dynamic> _clients;

        public ServerManager()
        {
            GameLoopCancellationSource = new CancellationTokenSource();
            CurrentGameManager = GetManager();
        }

        public void EnsureGameLoop(IHubCallerConnectionContext<dynamic> clients)
        {
            lock(_gameLoopLock)
            {
                if(_gameLoop == null)
                {
                    _clients = clients;
                    _gameLoop = GameLoop(GameLoopCancellationSource.Token);
                }
            }
        }

        private async Task GameLoop(CancellationToken token)
        {
            await Task.Yield();

            while(!token.IsCancellationRequested)
            {
                var viewModel = CurrentGameManager.GetCurrentLeague();
                if (viewModel != null)
                {
                    if (_clients != null)
                    {
                        _clients.All.updateRegionInfo(viewModel);
                    }
                }
                else
                {
                    await CurrentGameManager.SaveGameAsync();
                    CurrentGameManager = GetManagerForNewGame();
                    await CurrentGameManager.SaveGameAsync();
                }

                await Task.Delay(15);
            }

        }

        private GameManager GetManager()
        {
            var httpClient = new GameStateClient(new HttpClient(), "http://localhost:5000");
            var league = httpClient.GetCurrentLeagueAsync().Result;

            if (league == null)
            {
                var manager = GetManagerForNewGame();
                return manager;
            }

            var regions = httpClient.GetRegionsAsync(league.LeagueId).Result;
            var gameState = new GameState(league);
            foreach (var region in regions)
            {
                gameState.AddRegion(region);
            }


            return new GameManager(gameState, httpClient);
        }

        private GameManager GetManagerForNewGame()
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

            var gameState = new GameState(league);
            foreach (var region in regions)
            {
                gameState.AddRegion(region);
            }

            return new GameManager(gameState, httpClient);
        }
    }
}
