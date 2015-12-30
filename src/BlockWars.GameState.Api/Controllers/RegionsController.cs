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

        [HttpGet("api/realms/{realmId}/regions")]
        public async Task<IActionResult> GetRegionsAsync(string realmId)
        {
            //TODO: add validation attributes
            var regions = await _getRegionsService.GetRegionsAsync(realmId);
            return Ok(regions);
        }

        [HttpPut("api/realms/{realmId}/regions/{regionId}")]
        public async Task<IActionResult> PutRegionAsync(string realmId, string regionId, [FromBody] Region region)
        {
            //TODO: add validation attributes
            await _upsertRegionService.UpsertRegionAsync(realmId, regionId, region);
            return Ok();
        }

        [HttpPost("api/realms/{realmId}/regions/{regionId}/build_block")]
        public async Task<IActionResult> BuildBlockAsync(string realmId, string regionId, [FromBody]BuildRequest buildRequest)
        {
            //TODO: add validation attributes
            await _buildBlockService.BuildBlockAsync(regionId, buildRequest);
            return Ok();
        }

        [HttpPost("api/realms/{realmId}/regions/{regionId}/destroy_block")]
        public async Task<IActionResult> DestroyBlockAsync(string realmId, string regionId, [FromBody]DestroyRequest destroyRequest)
        {
            //TODO: Add validation attributes
            await _destroyBlockService.DestroyBlockAsync(regionId, destroyRequest);
            return Ok();
        }
    }
}
