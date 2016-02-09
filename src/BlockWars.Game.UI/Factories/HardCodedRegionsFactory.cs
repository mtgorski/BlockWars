using BlockWars.GameState.Models;
using System;
using System.Collections.Generic;

namespace BlockWars.Game.UI.Strategies
{
    public class HardCodedRegionsFactory : INewRegionsFactory
    {
        public ICollection<Region> GetRegions()
        {
            return new List<Region>
            {
                new Region
                {
                    Name = "Cats",
                    RegionId = Guid.NewGuid()
                },
                new Region
                {
                    Name = "Dogs",
                    RegionId = Guid.NewGuid()
                }
            };
        }
    }
}
