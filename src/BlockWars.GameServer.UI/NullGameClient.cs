using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BlockWars.GameState.Client;
using BlockWars.GameState.Models;

namespace BlockWars.Game.UI
{
    // Initial deployment to Azure won't include the GameState API. I found this the quickest way
    // to effectively toggle it off. 
    public class NullGameClient : IGameStateClient
    {
        public Task<League> GetCurrentLeagueAsync()
        {
            return Task.FromResult<League>(null);
        }

        public Task<ICollection<Region>> GetRegionsAsync(Guid leagueId)
        {
            throw new InvalidOperationException("GetRegionsAsync cannot be called on a NullGameClient.");
        }

        public Task PutLeagueAsync(Guid leagueId, League league)
        {
            return Task.FromResult(0);
        }

        public Task PutRegionAsync(Guid leagueId, Guid regionId, Region region)
        {
            return Task.FromResult(0);
        }
    }
}
