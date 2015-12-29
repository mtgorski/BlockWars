using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using BlockWars.GameState.Models;

namespace BlockWars.GameState.Api.Controllers
{
    [Route("api/realms")]
    public class RealmsController : Controller
    {
        private readonly IGetRealms _getRealmsService;
        private readonly IUpsertRealm _upsertRealmService;

        public RealmsController(IGetRealms getRealmsService, IUpsertRealm upsertRealmService)
        {
            _getRealmsService = getRealmsService;
            _upsertRealmService = upsertRealmService;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetRealmsAsync()
        {
            var realms = await _getRealmsService.GetRealmsAsync();
            return Ok(realms);
        }

        [HttpPut("{realmId}")]
        public async Task<IActionResult> PutRealmAsync(string realmId, [FromBody]Realm realm)
        {
            if(!ModelState.IsValid)
            {
                return HttpBadRequest(ModelState);
            }

            await _upsertRealmService.UpsertRealmAsync(realmId, realm);

            return Ok();
        } 
    }
}
