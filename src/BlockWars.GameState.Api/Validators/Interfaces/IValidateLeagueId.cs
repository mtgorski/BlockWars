using System;
using System.Threading.Tasks;

namespace BlockWars.GameState.Api.Validators.Interfaces
{
    public interface IValidateLeagueId
    {
        Task<bool> ValidateLeagueIdAsync(Guid leagueId);
    }
}
