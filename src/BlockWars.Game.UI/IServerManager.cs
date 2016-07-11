using BlockWars.Game.UI.Models;
using System;

namespace BlockWars.Game.UI
{
    public interface IServerManager
    {
        void BuildBlock(Guid leagueId, string regionName, string connectionId);

        void AddRegion(Guid leagueId, RegionState regionName, string connectionId);

        void AddConnectedUser(string connectionId);

        void RemoveConnectedUser(string connectionId);
    }
}