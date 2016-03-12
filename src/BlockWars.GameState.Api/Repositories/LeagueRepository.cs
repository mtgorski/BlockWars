using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using BlockWars.GameState.Api.DataModels;
using System;
using BlockWars.GameState.Api.Validators.Interfaces;

namespace BlockWars.GameState.Api.Repositories
{

    public class LeagueRepository : ILeagueRepository, IValidateLeagueId
    {
        private const string DatabaseName = "GameState";
        private const string CollectionName = "Leagues";

        private IMongoCollection<LeagueData> _leagues;

        public LeagueRepository(MongoClient client)
        {
            var database = client.GetDatabase(DatabaseName);
            _leagues = database.GetCollection<LeagueData>(CollectionName);
        }

        public async Task<ICollection<LeagueData>> GetLeaguesAsync(IQuery<LeagueData> query)
        {
            return await _leagues.Find(query.ToFilterDefinition()).ToListAsync();
        }

        public Task UpsertLeagueAsync(Guid leagueId, LeagueData league)
        {
            return _leagues.ReplaceOneAsync(
                r => r.LeagueId == leagueId, 
                league, 
                new UpdateOptions { IsUpsert = true });
        }

        public Task<bool> ValidateLeagueIdAsync(Guid leagueId)
        {
            return _leagues.Find(x => x.LeagueId == leagueId).AnyAsync();
        }
    }
}
