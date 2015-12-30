using BlockWars.GameState.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlockWars.GameState.Api
{
    public interface IBuildBlock
    {
        Task BuildBlockAsync(string regionId, BuildRequest request);
    }
}
