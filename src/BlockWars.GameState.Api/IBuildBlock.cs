using BlockWars.GameState.Models;
using System;
using System.Threading.Tasks;

namespace BlockWars.GameState.Api
{
    public interface IBuildBlock
    {
        Task BuildBlockAsync(Guid regionId, BuildRequest request);
    }
}
