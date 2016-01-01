using BlockWars.GameState.Models;
using System.Threading.Tasks;

namespace BlockWars.GameState.Api
{
    public interface IGameEngine
    {
        Task BuildBlockAsync(BuildBlockRequest request);
    }

    public class GameEngine
    {
        public async Task BuildBlockAsync(BuildBlockRequest request)
        {
            await Task.Yield();
            return;
        }
    }
}
