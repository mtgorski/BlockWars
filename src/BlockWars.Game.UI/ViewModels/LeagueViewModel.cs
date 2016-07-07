﻿using BlockWars.GameState.Models;
using System.Collections.Generic;

namespace BlockWars.Game.UI.ViewModels
{
    public class LeagueViewModel
    {
        public long RemainingMilliseconds { get; set; }
        public League League { get; set; }
        public ICollection<Region> Regions { get; set; } 
    }
}
