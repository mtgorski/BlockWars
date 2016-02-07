using BlockWars.GameState.Models;
using System;

namespace BlockWars.Game.UI
{
    public interface IServerManager
    {
        void BuildBlock(Guid leagueId, string regionName);

        void AddRegion(Guid leagueId, Region regionName);
    }
}