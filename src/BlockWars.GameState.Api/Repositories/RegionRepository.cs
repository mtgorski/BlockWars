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

    public class RegionRepository : IRegionRepository, IBuildBlock, IDestroyBlock
    {
        private readonly IMongoCollection<RegionData> _regions;
        private readonly IMongoDatabase _database;

        public RegionRepository()
        {
            var client = new MongoClient("mongodb://localhost:27017");
            _database = client.GetDatabase("GameState");
            _regions = _database.GetCollection<RegionData>("Regions");
        }

        public Task BuildBlockAsync(Guid regionId, BuildRequest request)
        {
            return GetBuiltCollection(regionId, request.Color).InsertOneAsync(new BsonDocument());
        }

        public Task DestroyBlockAsync(Guid regionId, DestroyRequest request)
        {
            return GetDestroyedCollection(regionId, request.Color).InsertOneAsync(new BsonDocument());
        }

        public async Task<ICollection<RegionData>> GetRegionsAsync(Guid leagueId)
        {
            var regions = await _regions.Find(x => x.LeagueId == leagueId).ToListAsync();

            foreach(var region in regions)
            {
                var blueBuilt = GetBuiltCollection(region.RegionId, BlockColor.Blue).Find(q => true).CountAsync();
                var blueDestroyed = GetDestroyedCollection(region.RegionId, BlockColor.Blue).Find(q => true).CountAsync();
                var redBuilt = GetBuiltCollection(region.RegionId, BlockColor.Red).Find(q => true).CountAsync();
                var redDestroyed = GetDestroyedCollection(region.RegionId, BlockColor.Red).Find(q => true).CountAsync();

                region.BlueCount = await blueBuilt - await blueDestroyed;
                region.RedCount = await redBuilt - await redDestroyed; 
            }

            return regions;
        }

        private IMongoCollection<BsonDocument> GetBuiltCollection(Guid regionId, BlockColor color)
        {
            return _database.GetCollection<BsonDocument>(regionId + "_" + color + "_Built");
        }

        private IMongoCollection<BsonDocument> GetDestroyedCollection(Guid regionId, BlockColor color)
        {
            return _database.GetCollection<BsonDocument>(regionId + "_" + color + "_Destroyed");
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
