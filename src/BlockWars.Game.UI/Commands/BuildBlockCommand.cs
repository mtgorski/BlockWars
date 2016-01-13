using System;

namespace BlockWars.Game.UI
{
    public class BuildBlockCommand
    {
        public Guid LeagueId { get; private set; }
        public string RegionName { get; private set; }

        public BuildBlockCommand(Guid leagueId, string regionName)
        {
            LeagueId = leagueId;
            RegionName = regionName;
        }
    }
}