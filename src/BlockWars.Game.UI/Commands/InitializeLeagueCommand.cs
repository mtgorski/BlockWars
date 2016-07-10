using BlockWars.Game.UI.Models;
using System.Collections.Generic;

namespace BlockWars.Game.UI.Commands
{
    public class InitializeLeagueCommand
    {
        public LeagueState LeagueData { get;  }
        public IEnumerable<RegionState> Regions { get; }

        public InitializeLeagueCommand(LeagueState league, IEnumerable<RegionState> regions)
        {
            LeagueData = league;
            Regions = regions;
        }
    }
}
