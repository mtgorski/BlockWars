using System;

namespace BlockWars.GameState.Models
{
    public class Region
    {
        public Guid RegionId { get; set; }

        public string Name { get; set; }

        public long RedCount { get; set; }

        public long BlueCount { get; set; }
    }
}
