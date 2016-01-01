using Microsoft.AspNet.Mvc;
using BlockWars.GameState.Models;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace BlockWars.GameState.Api.Controllers
{
    [Route("api/game_engine/build_block")]
    public class GameEngineController : Controller
    {
        private readonly IGameEngine _engine;

        public GameEngineController(IGameEngine engine)
        {
            _engine = engine;
        }

        [HttpPost]
        public async Task<IActionResult> BuildBlockAsync(BuildBlockRequest request)
        {
            await _engine.BuildBlockAsync(request);
            return Ok();
        }
    }
}
