using Microsoft.AspNet.Mvc;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

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
