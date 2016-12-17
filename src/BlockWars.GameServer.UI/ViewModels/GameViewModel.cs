using BlockWars.Game.UI.Actors;
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
        public ImmutableList<PlayerBlockCount> Players { get; }

        public GameViewModel(
            long remainingMilliseconds,
            Models.GameState game,
            IEnumerable<RegionState> regions,
            IEnumerable<PlayerBlockCount> players)
        {
            RemainingMilliseconds = remainingMilliseconds;
            Game = game;
            Regions = regions.ToImmutableList();
            Players = players.ToImmutableList();
        }
    }
}
