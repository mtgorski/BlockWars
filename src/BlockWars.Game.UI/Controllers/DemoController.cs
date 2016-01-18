using Microsoft.AspNet.Mvc;
using System;
using System.Linq;

namespace BlockWars.Game.UI.Controllers
{
    [Route("api/demo")]
    public class DemoController : Controller
    {
        private readonly IServerManager _serverManager;
        private static readonly Random Rng = new Random();

        public DemoController(IServerManager serverManager)
        {
            _serverManager = serverManager;
        }

        [HttpPost("build_block")]
        public IActionResult BuildBlock()
        {
            var currentLeague = _serverManager.GetCurrentLeagueView();
            if(currentLeague != null)
            {
                var whichRegionIndex = Rng.Next(currentLeague.Regions.Count);
                var whichRegion = currentLeague.Regions.Where((_, i) => i == whichRegionIndex).Single();
                _serverManager.BuildBlock(currentLeague.League.LeagueId, whichRegion.Name);
            }
            
            return Ok();
        }
    }
}
