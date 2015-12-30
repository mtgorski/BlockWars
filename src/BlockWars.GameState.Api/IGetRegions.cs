using BlockWars.GameState.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlockWars.GameState.Api
{
    public interface IGetRegions
    {
        Task<ICollection<Region>> GetRegionsAsync(string realmId);
    }
}
