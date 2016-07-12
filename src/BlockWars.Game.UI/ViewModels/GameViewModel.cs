using BlockWars.Game.UI.Models;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace BlockWars.Game.UI.ViewModels
{
    public struct GameViewModel
    {

        public long RemainingMilliseconds { get; }
        public Models.GameState Game { get; }
        public ImmutableList<RegionState> Regions{ get; }

        public GameViewModel(long remainingMilliseconds, Models.GameState game, IEnumerable<RegionState> regions)
        {
            RemainingMilliseconds = remainingMilliseconds;
            Game = game;
            Regions = regions.ToImmutableList();
        }
    }
}
