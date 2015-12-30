using BlockWars.GameState.Models;
using System;
using System.Threading.Tasks;

namespace BlockWars.GameState.Api
{
    public interface IDestroyBlock
    {
        Task DestroyBlockAsync(Guid regionId, DestroyRequest request);
    }
}
