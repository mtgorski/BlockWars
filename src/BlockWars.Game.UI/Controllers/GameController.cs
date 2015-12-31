using BlockWars.GameState.Client;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.SignalR.Infrastructure;
using System;
using System.Threading.Tasks;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace BlockWars.Game.UI.Controllers
{
    public class GameController : Controller
    {
        [HttpGet("leagues/{leagueId}")]
        public IActionResult League(Guid leagueId)
        {
            return View();
        }

        [HttpGet("hubtest")]
        public async Task<IActionResult> HubTest([FromServices]IConnectionManager manager)
        {
            var context = manager.GetHubContext<GameHub>();
            var client = new GameStateClient(new System.Net.Http.HttpClient(), "http://localhost:5000");
            var regionInfo = await client.GetRegionsAsync(Guid.Parse("5a44f339-3133-4f79-a6dc-1862568569cc"));
            context.Clients.All.updateRegionInfo(regionInfo);
            return Ok();
        }
    }
}
