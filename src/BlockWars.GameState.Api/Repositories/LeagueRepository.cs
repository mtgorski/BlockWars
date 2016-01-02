using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using BlockWars.GameState.Api.DataModels;
using System;

namespace BlockWars.GameState.Api.Repositories
{
    public interface ILeagueRepository
    {
        Task<ICollection<LeagueData>> GetLeaguesAsync(IQuery<LeagueData> query);
        Task UpsertLeagueAsync(Guid leagueId, LeagueData league);
    }

    public class LeagueRepository : ILeagueRepository
    {
        private IMongoCollection<LeagueData> _leagues;

        public LeagueRepository()
        {
            var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("GameState");
            _leagues = database.GetCollection<LeagueData>("Leagues");
        }

        public async Task<ICollection<LeagueData>> GetLeaguesAsync(IQuery<LeagueData> query)
        {
            return await _leagues.Find(query.ToFilterDefinition()).ToListAsync();
        }

        public Task UpsertLeagueAsync(Guid leagueId, LeagueData league)
        {
            return _leagues.FindOneAndReplaceAsync<LeagueData>(
                r => r.LeagueId == leagueId, 
                league, 
                new FindOneAndReplaceOptions<LeagueData, LeagueData> { IsUpsert = true });
        }
    }
}
