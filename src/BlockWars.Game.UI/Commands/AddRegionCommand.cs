using System;
using BlockWars.GameState.Models;

namespace BlockWars.Game.UI.Commands
{
    internal class AddRegionCommand
    {
        public Guid LeagueId { get; private set; }
        public Region Region { get; private set; }

        public AddRegionCommand(Guid leagueId, Region region)
        {
            LeagueId = leagueId;
            Region = region;
        }
    }
}