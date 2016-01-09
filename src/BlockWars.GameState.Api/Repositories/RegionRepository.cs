using BlockWars.GameState.Api.DataModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace BlockWars.GameState.Api.Repositories
{
    public interface IRegionRepository
    {
        Task<ICollection<RegionData>> GetRegionsAsync(Guid leagueId);
        Task UpsertRegionAsync(Guid regionId, RegionData regionData);
    }

    public class RegionRepository : IRegionRepository
    {
        private readonly IMongoCollection<RegionData> _regions;
        private readonly IMongoDatabase _database;

        public RegionRepository()
        {
            var client = new MongoClient("mongodb://localhost:27017");
            _database = client.GetDatabase("GameState");
            _regions = _database.GetCollection<RegionData>("Regions");
        }

        public async Task<ICollection<RegionData>> GetRegionsAsync(Guid leagueId)
        {
            return await _regions.Find(x => x.LeagueId == leagueId).ToListAsync();
        }

        public Task UpsertRegionAsync(Guid regionId, RegionData regionData)
        {
            return _regions
                .FindOneAndReplaceAsync<RegionData>(
                    x => x.RegionId == regionId,
                    regionData,
                    new FindOneAndReplaceOptions<RegionData, RegionData> { IsUpsert = true });
        }
    }
}
