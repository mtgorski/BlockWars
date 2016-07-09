using BlockWars.Game.UI.Actors;
using BlockWars.Game.UI.ViewModels;
using Microsoft.AspNet.Mvc;

namespace BlockWars.Game.UI.Controllers
{
    public class GameController : Controller
    {
        private AccomplishmentManager _accomplishmentManager;

        public GameController(AccomplishmentManager accomplishmentManager)
        {
            _accomplishmentManager = accomplishmentManager;
        }

        [HttpGet("")]
        public IActionResult Home()
        {
            return RedirectToAction("CurrentGame");
        }
       
        [HttpGet("games")]
        public IActionResult CurrentGame()
        {
            return View("League", new GamePageViewModel { NumberAccomplishments = _accomplishmentManager.GetCount() });
        }

    }
}
