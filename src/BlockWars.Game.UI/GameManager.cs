using BlockWars.Game.UI.ViewModels;
using BlockWars.GameState.Client;
using System.Threading.Tasks;
using BlockWars.GameState.Models;
using System;

namespace BlockWars.Game.UI
{
    public class GameManager
    {
        private readonly IGameStateClient _gameStateClient;
        private readonly IGameState _gameState;

        public GameManager(IGameState gameState, IGameStateClient gameStateClient)
        {
            _gameStateClient = gameStateClient;
            _gameState = gameState;    
        }

        public LeagueViewModel GetCurrentLeague()
        {
            if(_gameState.IsTheCurrentGame)
            {
                return _gameState.ToView();
            }

            return null;
        }

        public async Task SaveGameAsync()
        {
            var gameStateView = _gameState.ToView();
            await _gameStateClient.PutLeagueAsync(gameStateView.League.LeagueId, gameStateView.League);

            //TODO: refactor API so that this is not as chatty
            foreach(var region in gameStateView.Regions)
            {
                await _gameStateClient.PutRegionAsync(
                    gameStateView.League.LeagueId,
                    region.RegionId,
                    region);
            }
        }

        public void AddRegion(Guid leagueId, Region region)
        {
            ValidateLeagueId(leagueId);
            ValidateGameIsNotExpired("Cannot add a region to an expired game.");
            _gameState.AddRegion(region);
        }

        public void BuildBlock(Guid leagueId, string regionName)
        {
            ValidateLeagueId(leagueId);
            ValidateGameIsNotExpired("Cannot build a block when the game has expired.");

            _gameState.BuildBlock(regionName);
        }

        private void ValidateGameIsNotExpired(string message)
        {
            if (!_gameState.IsTheCurrentGame)
            {
                throw new InvalidOperationException(message);
            }
        }

        private void ValidateLeagueId(Guid leagueId)
        {
            if (leagueId != _gameState.LeagueId)
            {
                var leagueIdName = nameof(leagueId);
                throw new ArgumentException($"{leagueIdName} is not the leagueId of the current league.");
            }
        }
    }
}
