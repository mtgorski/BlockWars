using System;
using BlockWars.Game.UI.Models;

namespace BlockWars.Game.UI.Commands
{
    public class AddRegionCommand
    {
        public Guid GameId { get; private set; }
        public RegionState Region { get; private set; }

        public AddRegionCommand(Guid gameId, RegionState region)
        {
            GameId = gameId;
            Region = region;
        }
    }
}