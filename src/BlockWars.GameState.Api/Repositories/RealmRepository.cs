using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using BlockWars.GameState.Api.DataModels;

namespace BlockWars.GameState.Api.Repositories
{
    public interface IRealmRepository
    {
        Task<ICollection<RealmData>> GetRealmsAsync();
        Task UpsertRealmAsync(string realmId, RealmData realm);
    }

    public class RealmRepository : IRealmRepository
    {
        private IMongoCollection<RealmData> _realms;

        public RealmRepository()
        {
            var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("GameState");
            _realms = database.GetCollection<RealmData>("Realms");
        }

        public async Task<ICollection<RealmData>> GetRealmsAsync()
        {
            return await _realms.Find(q => true).ToListAsync();
        }

        public Task UpsertRealmAsync(string realmId, RealmData realm)
        {
            return _realms.FindOneAndReplaceAsync<RealmData>(
                r => r.RealmId == realmId, 
                realm, 
                new FindOneAndReplaceOptions<RealmData, RealmData> { IsUpsert = true });
        }
    }
}
