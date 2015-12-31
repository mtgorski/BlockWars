using BlockWars.GameState.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlockWars.GameState.Client
{
    public interface IGameStateClient
    {
        Task<ICollection<League>> GetLeaguesAsync();

        Task PutLeagueAsync(Guid leagueId, League league);

        Task<ICollection<Region>> GetRegionsAsync(Guid leagueId);

        Task PutRegionAsync(Guid leagueId, Guid regionId, Region region);

        Task BuildBlockAsync(Guid leagueId, Guid regionId, BuildRequest buildRequest);
        
        Task DestroyBlockAsync(Guid leagueId, Guid regionId, DestroyRequest destroyRequest);
    }
}
