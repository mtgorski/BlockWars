using BlockWars.GameState.Api.DataModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace BlockWars.GameState.Api.Repositories
{

    public class RegionRepository : IRegionRepository
    {
        private const string DatabaseName = "GameState";
        private const string CollectionName = "Regions";

        private readonly IMongoCollection<RegionData> _regions;

        public RegionRepository(MongoClient client)
        {
            var database = client.GetDatabase(DatabaseName);
            _regions = database.GetCollection<RegionData>(CollectionName);
        }

        public async Task<ICollection<RegionData>> GetRegionsAsync(Guid leagueId)
        {
            return await _regions.Find(x => x.LeagueId == leagueId).ToListAsync();
        }

        public Task UpsertRegionAsync(Guid regionId, RegionData regionData)
        {
            return _regions
                .ReplaceOneAsync(
                    x => x.RegionId == regionId,
                    regionData,
                    new UpdateOptions { IsUpsert = true });
        }
    }
}
