using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using BlockWars.GameState.Models;
using System;

namespace BlockWars.GameState.Api.Controllers
{
    [Route("api/leagues")]
    public class LeaguesController : Controller
    {
        private readonly IGetLeagues _getLeaguesService;
        private readonly IUpsertLeague _upsertLeagueService;

        public LeaguesController(IGetLeagues getLeaguesService, IUpsertLeague upsertLeagueService)
        {
            _getLeaguesService = getLeaguesService;
            _upsertLeagueService = upsertLeagueService;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetLeaguesAsync()
        {
            var leagues = await _getLeaguesService.GetLeaguesAsync();
            return Ok(leagues);
        }

        [HttpPut("{leagueId}")]
        public async Task<IActionResult> PutLeagueAsync(Guid leagueId, [FromBody]League league)
        {
            if(!ModelState.IsValid)
            {
                return HttpBadRequest(ModelState);
            }

            await _upsertLeagueService.UpsertLeagueAsync(leagueId, league);

            return Ok();
        } 
    }
}
