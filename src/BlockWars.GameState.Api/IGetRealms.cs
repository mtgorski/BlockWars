using BlockWars.GameState.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlockWars.GameState.Api
{
    public interface IGetRealms
    {
        Task<ICollection<Realm>> GetRealmsAsync();
    }
}
