using System.Threading.Tasks;
using Xunit;

namespace BlockWars.GameState.Api.Unit.Tests.Validators
{
    public class LeagueIdValidatorTests
    {
        [Theory, AutoMoq]
        public async Task Validate_GivenValidId_ShouldReturnTrue()
        {
            await Task.Yield();
        }
    }
}
