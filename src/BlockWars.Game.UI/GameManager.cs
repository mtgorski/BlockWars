using BlockWars.Game.UI.ViewModels;
using BlockWars.GameState.Client;
using System.Threading.Tasks;
using BlockWars.GameState.Models;

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

        public void AddRegion(Region region)
        {
            if(_gameState.IsTheCurrentGame)
            {
                _gameState.AddRegion(region);
            }
        }

        public void BuildBlock(string regionName)
        {
            if(_gameState.IsTheCurrentGame)
            {
                _gameState.BuildBlock(regionName);
            }
        }
    }
}
