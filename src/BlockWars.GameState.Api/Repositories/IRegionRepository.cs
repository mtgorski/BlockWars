using BlockWars.GameState.Api.DataModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlockWars.GameState.Api.Repositories
{
    public interface IRegionRepository
    {
        Task<ICollection<RegionData>> GetRegionsAsync(Guid leagueId);
        Task UpsertRegionAsync(Guid regionId, RegionData regionData);
    }
}
