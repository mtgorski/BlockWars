﻿using System;
using System.Collections.Generic;

namespace BlockWars.Game.UI.Actors
{
    public class AccomplishmentManager
    {
        private Dictionary<int, Accomplishment> _thresholdToAccomplishment = new Dictionary<int, Accomplishment>
        {
            {1, new Accomplishment { Text = "I - Baby Builder", Rank = 1} },
            {10, new Accomplishment { Text = "II - Lego Builder", Rank = 2} },
            {50, new Accomplishment { Text = "III - Mason", Rank = 3 } },
            {100, new Accomplishment { Text = "IV - Master Builder", Rank = 4 } },
            {314, new Accomplishment { Text = "V - Master Baker", Rank = 5} },
            {500, new Accomplishment { Text = "VI - Monument Maker", Rank = 6 } },
            {1000, new Accomplishment { Text = "VII - World Builder", Rank = 7} }
        };

        public int GetCount()
        {
            return _thresholdToAccomplishment.Count;
        }

        public Accomplishment GetAccomplishment(int blockCount)
        {
            if(_thresholdToAccomplishment.ContainsKey(blockCount))
            {
                return _thresholdToAccomplishment[blockCount];
            }

            return null;  
        }
    }
}