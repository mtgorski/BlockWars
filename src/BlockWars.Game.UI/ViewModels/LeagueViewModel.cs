using BlockWars.Game.UI.Models;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace BlockWars.Game.UI.ViewModels
{
    public struct LeagueViewModel
    {

        public long RemainingMilliseconds { get; }
        public LeagueState League { get; }
        public ImmutableList<RegionState> Regions{ get; }

        public LeagueViewModel(long remainingMilliseconds, LeagueState league, IEnumerable<RegionState> regions)
        {
            RemainingMilliseconds = remainingMilliseconds;
            League = league;
            Regions = regions.ToImmutableList();
        }
    }
}
