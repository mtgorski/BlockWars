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
        private readonly IBuildBlock _buildBlockService;
        private readonly IDestroyBlock _destroyBlockService;

        public RegionsController(IGetRegions getRegionsService, IUpsertRegion upsertRegionService, IBuildBlock buildBlockService, IDestroyBlock destroyBlockService)
        {
            _getRegionsService = getRegionsService;
            _upsertRegionService = upsertRegionService;
            _buildBlockService = buildBlockService;
            _destroyBlockService = destroyBlockService;
        }

        [HttpGet("api/leagues/{leagueId}/regions")]
        public async Task<IActionResult> GetRegionsAsync(Guid leagueId)
        {
            //TODO: add validation attributes
            var regions = await _getRegionsService.GetRegionsAsync(leagueId);
            return Ok(regions);
        }

        [HttpPut("api/leagues/{leagueId}/regions/{regionId}")]
        public async Task<IActionResult> PutRegionAsync(Guid leagueId, Guid regionId, [FromBody] Region region)
        {
            //TODO: add validation attributes
            await _upsertRegionService.UpsertRegionAsync(leagueId, regionId, region);
            return Ok();
        }

        [HttpPost("api/leagues/{leagueId}/regions/{regionId}/build_block")]
        public async Task<IActionResult> BuildBlockAsync(Guid leagueId, Guid regionId, [FromBody]BuildRequest buildRequest)
        {
            //TODO: add validation attributes
            await _buildBlockService.BuildBlockAsync(regionId, buildRequest);
            return Ok();
        }

        [HttpPost("api/leagues/{leagueId}/regions/{regionId}/destroy_block")]
        public async Task<IActionResult> DestroyBlockAsync(Guid leagueId, Guid regionId, [FromBody]DestroyRequest destroyRequest)
        {
            //TODO: Add validation attributes
            await _destroyBlockService.DestroyBlockAsync(regionId, destroyRequest);
            return Ok();
        }
    }
}
