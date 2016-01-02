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
       
        [HttpGet("games")]
        public IActionResult CurrentGame()
        {
            return View("League");
        }

    }
}
