using System;
using System.Threading.Tasks;
using BlockWars.GameState.Models;

namespace BlockWars.GameState.Api.Services
{
    public class BuildBlockService : IBuildBlock
    {
        public Task BuildBlockAsync(Guid regionId, BuildRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
