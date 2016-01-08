using BlockWars.Game.UI.ViewModels;
using BlockWars.GameState.Client;
using BlockWars.GameState.Models;
using FluentAssertions;
using Moq;
using Ploeh.AutoFixture.Xunit2;
using System;
using System.Threading.Tasks;
using Xunit;

namespace BlockWars.Game.UI.Unit.Tests
{
    public class GameManagerTests
    {
        [Theory, AutoMoq]
        public void GetCurrentLeague_GivenTheInMemoryStateIsCurrent_ReturnTheLeagueView(
            [Frozen] Mock<IGameState> gameState,
            [Frozen] Mock<IGameStateClient> client,
            LeagueViewModel view)
        {
            gameState.Setup(m => m.IsTheCurrentGame).Returns(true);
            gameState.Setup(m => m.ToView()).Returns(view);
            var sut = new GameManager(gameState.Object, client.Object);
            
            var actual = sut.GetCurrentLeague();

            actual.Should().BeSameAs(view);
        }

        [Theory, AutoMoq]
        public void GetCurrentLeague_GivenTheInMemoryStateIsNotCurrent_ReturnNull(
            [Frozen] Mock<IGameState> gameState,
            [Frozen] Mock<IGameStateClient> client
            )
        {
            gameState.Setup(m => m.IsTheCurrentGame).Returns(false);
            var sut = new GameManager(gameState.Object, client.Object);

            var actual = sut.GetCurrentLeague();

            actual.Should().BeNull();
        }

        [Theory, AutoMoq]
        public async Task SaveGame_ShouldSaveTheLeague(
            [Frozen] Mock<IGameState> gameState,
            [Frozen] Mock<IGameStateClient> gameClient,
            LeagueViewModel gameStateView,
            GameManager sut)
        {
            gameState.Setup(m => m.ToView()).Returns(gameStateView);
            gameClient.Setup(m => m.PutLeagueAsync(gameStateView.League.LeagueId, gameStateView.League))
                .Returns(Task.FromResult(0)).Verifiable();

            await sut.SaveGameAsync();

            gameClient.Verify();
        }

        [Theory, AutoMoq]
        public async Task SaveGame_ShouldSaveTheRegions(
            [Frozen] Mock<IGameState> gameState,
            [Frozen] Mock<IGameStateClient> gameClient,
            LeagueViewModel gameStateView,
            GameManager sut)
        {
            gameState.Setup(m => m.ToView()).Returns(gameStateView);
            foreach(var region in gameStateView.Regions)
            {
                gameClient.Setup(
                    m => m.PutRegionAsync(
                            gameStateView.League.LeagueId,
                            region.RegionId,
                            region)
                   ).Returns(Task.FromResult(0)).Verifiable();
            }

            await sut.SaveGameAsync();

            gameClient.Verify();
        }

        [Theory, AutoMoq]
        public void AddRegion_GivenTheGameStateIsCurrent_ShouldAddRegion(
            [Frozen]Mock<IGameState> gameState,
            Region givenRegion,
            GameManager sut)
        {
            gameState.Setup(m => m.IsTheCurrentGame).Returns(true);
            gameState.Setup(m => m.AddRegion(givenRegion)).Verifiable();

            sut.AddRegion(givenRegion);

            gameState.Verify();
        }

        [Theory, AutoMoq]
        public void AddRegion_GivenTheGameStateIsNotCurrent_ShouldNotAddRegion(
            [Frozen]Mock<IGameState> gameState,
            Region givenRegion,
            GameManager sut)
        {
            gameState.Setup(m => m.IsTheCurrentGame).Returns(false);

            sut.AddRegion(givenRegion);

            gameState.Verify(m => m.AddRegion(It.IsAny<Region>()), Times.Never);
        }

        [Theory, AutoMoq]
        public void BuildBlock_GivenTheGameStateIsCurrent_ShouldBuildBlock(
            [Frozen]Mock<IGameState> gameState,
            string regionName,
            GameManager sut)
        {
            gameState.Setup(m => m.IsTheCurrentGame).Returns(true);
            gameState.Setup(m => m.BuildBlock(regionName)).Verifiable();

            sut.BuildBlock(regionName);

            gameState.Verify();
        }

        [Theory, AutoMoq]
        public void BuildBlock_GivenTheGameStateIsNotCurrent_ShouldNotBuildBlock(
            [Frozen]Mock<IGameState> gameState,
            string regionName,
            GameManager sut)
        {
            gameState.Setup(m => m.IsTheCurrentGame).Returns(false);

            sut.BuildBlock(regionName);

            gameState.Verify(m => m.BuildBlock(regionName), Times.Never);
        }
    }
}
