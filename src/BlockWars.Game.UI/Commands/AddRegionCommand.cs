using System;
using BlockWars.Game.UI.Models;

namespace BlockWars.Game.UI.Commands
{
    public class AddRegionCommand
    {
        public Guid LeagueId { get; private set; }
        public RegionState Region { get; private set; }

        public AddRegionCommand(Guid leagueId, RegionState region)
        {
            LeagueId = leagueId;
            Region = region;
        }
    }
}