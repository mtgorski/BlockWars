using BlockWars.GameState.Models;
using System.Collections.Generic;

namespace BlockWars.Game.UI.Strategies
{
    public interface INewRegionsFactory
    {
        ICollection<Region> GetRegions();
    }
}
