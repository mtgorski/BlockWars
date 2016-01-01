using System;

namespace BlockWars.GameState.Models
{
    public class Region
    {
        public Guid RegionId { get; set; }

        public string Name { get; set; }

        public long BlockCount { get; set; }
    }
}
