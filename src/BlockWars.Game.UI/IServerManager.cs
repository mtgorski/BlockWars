using BlockWars.Game.UI.Models;
using System;

namespace BlockWars.Game.UI
{
    public interface IServerManager
    {
        void BuildBlock(Guid leagueId, string regionName, string connectionId);

        void AddRegion(Guid leagueId, RegionState regionName);

        void AddStatsActor(string connectionId);
        void RemoveStatsActor(string connectionId);
    }
}