using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlockWars.GameState.Models
{
    public class Region
    {
        public string RegionId { get; set; }

        public string Name { get; set; }

        public long RedCount { get; set; }

        public long BlueCount { get; set; }
    }
}
