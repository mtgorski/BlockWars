using BlockWars.GameState.Models;
using System.Threading.Tasks;

namespace BlockWars.GameState.Api
{
    public interface IUpsertRegion
    {
        Task UpsertRegionAsync(string leagueId, string regionId, Region region);
    }
}
