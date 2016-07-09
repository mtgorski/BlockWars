using Microsoft.AspNet.Mvc;

namespace BlockWars.Game.UI.Controllers
{
    public class GameController : Controller
    {
        [HttpGet("")]
        public IActionResult Home()
        {
            return RedirectToAction("CurrentGame");
        }
       
        [HttpGet("games")]
        public IActionResult CurrentGame()
        {
            return View("League");
        }

    }
}
