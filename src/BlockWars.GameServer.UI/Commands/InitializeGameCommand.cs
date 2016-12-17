using BlockWars.Game.UI.Models;
using System.Collections.Generic;

namespace BlockWars.Game.UI.Commands
{
    public class InitializeGameCommand
    {
        public Models.GameState GameData { get;  }
        public IEnumerable<RegionState> Regions { get; }

        public InitializeGameCommand(Models.GameState game, IEnumerable<RegionState> regions)
        {
            GameData = game;
            Regions = regions;
        }
    }
}
