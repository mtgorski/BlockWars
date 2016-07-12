using System;

namespace BlockWars.Game.UI
{
    public class BuildBlockCommand
    {
        public Guid GameId { get; }
        public string RegionName { get; }
        public string ConnectionId { get; }

        public BuildBlockCommand(Guid gameId, string regionName, string connectionId)
        {
            GameId = gameId;
            RegionName = regionName;
            ConnectionId = connectionId;
        }
    }
}