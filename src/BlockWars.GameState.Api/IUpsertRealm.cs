using BlockWars.GameState.Models;
using System.Threading.Tasks;

namespace BlockWars.GameState.Api
{
    public interface IUpsertRealm
    {
        Task UpsertRealmAsync(string realmId, Realm realm);
    }
}
