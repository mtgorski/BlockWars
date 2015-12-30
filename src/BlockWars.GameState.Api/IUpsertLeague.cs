using BlockWars.GameState.Models;
using System.Threading.Tasks;

namespace BlockWars.GameState.Api
{
    public interface IUpsertLeague
    {
        Task UpsertLeagueAsync(string leagueId, League league);
    }
}
