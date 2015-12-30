using BlockWars.GameState.Models;
using System;
using System.Threading.Tasks;

namespace BlockWars.GameState.Api
{
    public interface IUpsertRegion
    {
        Task UpsertRegionAsync(Guid leagueId, Guid regionId, Region region);
    }
}
