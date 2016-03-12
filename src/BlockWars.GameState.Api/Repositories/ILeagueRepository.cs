using BlockWars.GameState.Api.DataModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlockWars.GameState.Api.Repositories
{
    public interface ILeagueRepository
    {
        Task<ICollection<LeagueData>> GetLeaguesAsync(IQuery<LeagueData> query);
        Task UpsertLeagueAsync(Guid leagueId, LeagueData league);
    }
}
