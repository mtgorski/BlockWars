using System.Threading.Tasks;
using Akka.Actor;
using BlockWars.GameState.Client;

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
            await _client.PutLeagueAsync(x.FinalState.League.LeagueId, x.FinalState.League);

            // TODO: make this less chatty by adding a bulk API
            foreach(var region in x.FinalState.Regions)
            {
                await _client.PutRegionAsync(x.FinalState.League.LeagueId, region.RegionId, region);
            }
        }
    }
}
