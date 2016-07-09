using BlockWars.Game.UI.Models;
using System.Collections.Generic;

namespace BlockWars.Game.UI.ViewModels
{
    public class LeagueViewModel
    {
        public long RemainingMilliseconds { get; set; }
        public LeagueState League { get; set; }
        public ICollection<RegionState> Regions { get; set; } 
    }
}
