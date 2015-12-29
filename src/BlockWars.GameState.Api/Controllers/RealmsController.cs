using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using BlockWars.GameState.Models;

namespace BlockWars.GameState.Api.Controllers
{
    [Route("api/realms")]
    public class RealmsController : Controller
    {
        private readonly IGetRealms _getRealmsService;

        public RealmsController(IGetRealms getRealmsService)
        {
            _getRealmsService = getRealmsService;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetRealms()
        {
            var realms = await _getRealmsService.GetRealmsAsync();
            return Ok(realms);
        }
    }
}
