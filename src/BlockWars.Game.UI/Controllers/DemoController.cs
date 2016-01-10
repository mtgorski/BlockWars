using Microsoft.AspNet.Mvc;
using System;
using System.Linq;

namespace BlockWars.Game.UI.Controllers
{
    [Route("api/demo")]
    public class DemoController : Controller
    {
        private readonly GameManager _gameManager;
        private static readonly Random Rng = new Random();

        public DemoController(IServerManager serverManager)
        {
            _gameManager = serverManager.CurrentGameManager;
        }

        [HttpPost("build_block")]
        public IActionResult BuildBlock()
        {
            var currentLeague = _gameManager?.GetCurrentLeague();
            if(currentLeague != null)
            {
                var whichRegionIndex = Rng.Next(currentLeague.Regions.Count);
                var whichRegion = currentLeague.Regions.Where((_, i) => i == whichRegionIndex).Single();
                _gameManager.BuildBlock(currentLeague.League.LeagueId, whichRegion.Name);
            }

            return Ok();
        }
    }
}
