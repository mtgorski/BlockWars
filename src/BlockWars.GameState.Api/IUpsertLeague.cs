using BlockWars.GameState.Models;
using System;
using System.Threading.Tasks;

namespace BlockWars.GameState.Api
{
    public interface IUpsertLeague
    {
        Task UpsertLeagueAsync(Guid leagueId, League league);
    }
}
