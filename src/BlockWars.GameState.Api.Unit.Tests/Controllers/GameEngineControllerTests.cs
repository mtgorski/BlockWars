using BlockWars.GameState.Api.Controllers;
using BlockWars.GameState.Models;
using FluentAssertions;
using Microsoft.AspNet.Mvc;
using Moq;
using Ploeh.AutoFixture.Xunit2;
using System.Threading.Tasks;
using Xunit;

namespace BlockWars.GameState.Api.Unit.Tests.Controllers
{
    public class GameEngineControllerTests
    {
        [Theory, AutoMoqController]
        public async Task BuildBlockAsync_ShouldUseEngineToBuildBlock(
            [Frozen]Mock<IGameEngine> gameEngine,
            BuildBlockRequest givenRequest,
            GameEngineController sut)
        {
            gameEngine.Setup(m => m.BuildBlockAsync(givenRequest)).Returns(Task.FromResult(0)).Verifiable();

            var actual = await sut.BuildBlockAsync(givenRequest);

            actual.Should().BeAssignableTo<HttpOkResult>();
            gameEngine.Verify();
        }
    }
}
