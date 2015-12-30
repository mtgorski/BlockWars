using BlockWars.GameState.Models;
using System.Threading.Tasks;

namespace BlockWars.GameState.Api
{
    public interface IDestroyBlock
    {
        Task DestroyBlockAsync(string regionId, DestroyRequest request);
    }
}
