using Microsoft.AspNet.Mvc;
using System.Threading.Tasks;
using BlockWars.GameState.Models;
using System;
using BlockWars.GameState.Api.Filters;

namespace BlockWars.GameState.Api.Controllers
{
    public class RegionsController : Controller
    {
        private readonly IGetRegions _getRegionsService;
        private readonly IUpsertRegion _upsertRegionService;

        public RegionsController(IGetRegions getRegionsService, IUpsertRegion upsertRegionService)
        {
            _getRegionsService = getRegionsService;
            _upsertRegionService = upsertRegionService;
        }

        [ServiceFilter(typeof(ValidateLeagueIdFilter))]
        [HttpGet("api/leagues/{leagueId}/regions")]
        public async Task<IActionResult> GetRegionsAsync(Guid leagueId)
        {
            var regions = await _getRegionsService.GetRegionsAsync(leagueId);
            var response = new RegionsResponse { Regions = regions };
            return Ok(response);
        }

        [ServiceFilter(typeof(ValidateLeagueIdFilter))]
        [ServiceFilter(typeof(ValidateRegionFilter))]
        [HttpPut("api/leagues/{leagueId}/regions/{regionId}")]
        public async Task<IActionResult> PutRegionAsync(Guid leagueId, Guid regionId, [FromBody] Region region)
        {
            await _upsertRegionService.UpsertRegionAsync(leagueId, regionId, region);
            return Ok();
        }

    }
}
