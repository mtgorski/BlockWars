using BlockWars.GameState.Client;
using System.Threading.Tasks;

namespace BlockWars.Game.UI
{
    public class GameManagerProvider : IGameManagerProvider
    {
        private readonly IGameStateClient _gameStateClient;
        private readonly INewInstanceFactory _newInstanceFactory;

        public GameManagerProvider(IGameStateClient gameStateClient, INewInstanceFactory newInstanceFactory)
        {
            _gameStateClient = gameStateClient;
            _newInstanceFactory = newInstanceFactory;
        }

        public GameManager GetFromNewGame()
        {
            var gameState = _newInstanceFactory.GetInstance();
            
            return new GameManager(gameState, _gameStateClient);
        }


        public async Task<GameManager> GetFromSavedGameAsync()
        {
            var league = await _gameStateClient.GetCurrentLeagueAsync();
            if(league == null)
            {
                return null;
            }

            var gameState = new GameState(league);

            var regions = await _gameStateClient.GetRegionsAsync(league.LeagueId);
            
            foreach (var region in regions)
            {
                gameState.AddRegion(region);
            }

            return new GameManager(gameState, _gameStateClient);
        }
    }
}