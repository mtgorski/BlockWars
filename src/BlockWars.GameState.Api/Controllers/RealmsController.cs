using System.Collections.Generic;
using Microsoft.AspNet.Mvc;


namespace BlockWars.GameState.Api.Controllers
{
    [Route("api/[controller]")]
    public class RealmsController : Controller
    {
        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

    }
}
