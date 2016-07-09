using BlockWars.Game.UI.Models;
using System.Collections.Generic;

namespace BlockWars.Game.UI.Strategies
{
    public class HardCodedRegionsFactory : INewRegionsFactory
    {
        public ICollection<RegionState> GetRegions()
        {
            return new List<RegionState>
            {
                new RegionState("Cats"),
                new RegionState("Dogs")
            };
        }
    }
}
