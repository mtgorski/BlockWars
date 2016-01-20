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

            Receive<SaveLeagueCommand>(async x =>
            {
                await SaveLeagueAsync(x);               
            });
        }

        private async Task SaveLeagueAsync(SaveLeagueCommand x)
        {
            await _client.PutLeagueAsync(x.ViewModel.League.LeagueId, x.ViewModel.League);

            foreach(var region in x.ViewModel.Regions)
            {
                await _client.PutRegionAsync(x.ViewModel.League.LeagueId, region.RegionId, region);
            }
            Sender.Tell(new SavedLeagueMessage());
        }
    }
}
