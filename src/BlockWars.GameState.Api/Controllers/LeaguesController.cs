using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using BlockWars.GameState.Models;
using System;
using BlockWars.GameState.Api.Attributes;

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
        public async Task<IActionResult> GetLeaguesAsync(LeagueSearchRequest request)
        {
            var leagues = await _getLeaguesService.GetLeaguesAsync(request);
            var response = new LeaguesResponse { Leagues = leagues };
            return Ok(response);
        }

        [HttpPut("{leagueId}")]
        [ServiceFilter(typeof(ValidateLeagueFilter))]
        public async Task<IActionResult> PutLeagueAsync(Guid leagueId, [FromBody]League league)
        {
            await _upsertLeagueService.UpsertLeagueAsync(leagueId, league);

            return Ok();
        } 
    }
}
