using BlockWars.Game.UI.Models;
using System;

namespace BlockWars.Game.UI
{
    public interface IServerManager
    {
        void BuildBlock(Guid leagueId, string regionName);

        void AddRegion(Guid leagueId, RegionState regionName);
    }
}