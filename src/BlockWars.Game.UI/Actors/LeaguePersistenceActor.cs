using System.Threading.Tasks;
using Akka.Actor;
using BlockWars.GameState.Client;
using BlockWars.GameState.Models;

namespace BlockWars.Game.UI.Actors
{
    public class LeaguePersistenceActor : ReceiveActor
    {
        private readonly IGameStateClient _client;
         
        public LeaguePersistenceActor(IGameStateClient client)
        {
            _client = client;

            Receive<LeagueEndedMessage>(async x =>
            {
                await SaveLeagueAsync(x);               
            });
        }

        private async Task SaveLeagueAsync(LeagueEndedMessage x)
        {
            var league = new League
            {
                CreatedAt = x.FinalState.League.CreatedAt,
                Description = x.FinalState.League.Description,
                Duration = x.FinalState.League.Duration,
                LeagueId = x.FinalState.League.LeagueId,
                Name = x.FinalState.League.Name
            };
            await _client.PutLeagueAsync(x.FinalState.League.LeagueId, league);

            // TODO: make this less chatty by adding a bulk API
            foreach(var region in x.FinalState.Regions)
            {
                var apiRegion = new Region
                {
                    BlockCount = region.BlockCount,
                    Name = region.Name,
                    RegionId = region.RegionId
                };
                await _client.PutRegionAsync(x.FinalState.League.LeagueId, apiRegion.RegionId, apiRegion);
            }
        }
    }
}
