using System.Threading.Tasks;
using Akka.Actor;
using BlockWars.GameState.Client;
using BlockWars.GameState.Models;

namespace BlockWars.Game.UI.Actors
{
    public class GamePersistenceActor : ReceiveActor
    {
        private readonly IGameStateClient _client;
         
        public GamePersistenceActor(IGameStateClient client)
        {
            _client = client;

            Receive<GameEndedMessage>(async x =>
            {
                await SaveGameAsync(x);               
            });
        }

        private async Task SaveGameAsync(GameEndedMessage x)
        {
            var league = new League
            {
                CreatedAt = x.FinalState.Game.CreatedAt,
                Description = x.FinalState.Game.Description,
                Duration = x.FinalState.Game.Duration,
                LeagueId = x.FinalState.Game.GameId,
                Name = x.FinalState.Game.Name
            };
            await _client.PutLeagueAsync(x.FinalState.Game.GameId, league);

            // TODO: make this less chatty by adding a bulk API
            foreach(var region in x.FinalState.Regions)
            {
                var apiRegion = new Region
                {
                    BlockCount = region.BlockCount,
                    Name = region.Name,
                    RegionId = region.RegionId
                };
                await _client.PutRegionAsync(x.FinalState.Game.GameId, apiRegion.RegionId, apiRegion);
            }
        }
    }
}
