using BlockWars.GameState.Api.DataModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BlockWars.GameState.Models;
using MongoDB.Driver;
using MongoDB.Bson;

namespace BlockWars.GameState.Api.Repositories
{
    public interface IRegionRepository
    {
        Task<ICollection<RegionData>> GetRegionsAsync(Guid leagueId);
        Task UpsertRegionAsync(Guid regionId, RegionData regionData);
    }

    public class RegionRepository : IRegionRepository, IBuildBlock
    {
        private readonly IMongoCollection<RegionData> _regions;
        private readonly IMongoDatabase _database;

        public RegionRepository()
        {
            var client = new MongoClient("mongodb://localhost:27017");
            _database = client.GetDatabase("GameState");
            _regions = _database.GetCollection<RegionData>("Regions");
        }

        public Task BuildBlockAsync(Guid regionId)
        {
            return GetBuiltCollection(regionId).InsertOneAsync(new BsonDocument());
        }

        public async Task<ICollection<RegionData>> GetRegionsAsync(Guid leagueId)
        {
            var regions = await _regions.Find(x => x.LeagueId == leagueId).ToListAsync();

            foreach(var region in regions)
            {
                var built = GetBuiltCollection(region.RegionId).Find(q => true).CountAsync();

                region.BlockCount = await built;
            }

            return regions;
        }

        private IMongoCollection<BsonDocument> GetBuiltCollection(Guid regionId)
        {
            return _database.GetCollection<BsonDocument>(regionId + "_Built");
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
