using BlockWars.Game.UI.Models;
using System;

namespace BlockWars.Game.UI
{
    public interface IServerManager
    {
        void BuildBlock(Guid gameId, string regionName, string connectionId);

        void AddRegion(Guid gameId, RegionState regionName, string connectionId);

        void AddConnectedUser(string connectionId);

        void RemoveConnectedUser(string connectionId);

        void ChangeName(string connectionId, string name);
    }
}