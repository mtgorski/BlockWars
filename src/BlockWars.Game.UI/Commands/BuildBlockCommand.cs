using System;

namespace BlockWars.Game.UI
{
    public class BuildBlockCommand
    {
        public Guid LeagueId { get; }
        public string RegionName { get; }
        public string ConnectionId { get; }

        public BuildBlockCommand(Guid leagueId, string regionName, string connectionId)
        {
            LeagueId = leagueId;
            RegionName = regionName;
            ConnectionId = connectionId;
        }
    }
}