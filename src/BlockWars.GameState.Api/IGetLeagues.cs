using BlockWars.GameState.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlockWars.GameState.Api
{
    public interface IGetLeagues
    {
        Task<ICollection<League>> GetLeaguesAsync(LeagueSearchRequest request);
    }
}
