using BlockWars.Game.UI.Models;
using System.Collections.Generic;

namespace BlockWars.Game.UI.Strategies
{
    public interface INewRegionsFactory
    {
        ICollection<RegionState> GetRegions();
    }
}
