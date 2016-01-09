using Microsoft.AspNet.Mvc;
using System.Threading.Tasks;
using BlockWars.GameState.Models;
using System;

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

        [HttpGet("api/leagues/{leagueId}/regions")]
        public async Task<IActionResult> GetRegionsAsync(Guid leagueId)
        {
            //TODO: add validation attributes
            var regions = await _getRegionsService.GetRegionsAsync(leagueId);
            var response = new RegionsResponse { Regions = regions };
            return Ok(response);
        }

        [HttpPut("api/leagues/{leagueId}/regions/{regionId}")]
        public async Task<IActionResult> PutRegionAsync(Guid leagueId, Guid regionId, [FromBody] Region region)
        {
            //TODO: add validation attributes
            await _upsertRegionService.UpsertRegionAsync(leagueId, regionId, region);
            return Ok();
        }

    }
}
