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
            Guid currentLeagueId,
            Region givenRegion,
            GameManager sut)
        {
            gameState.Setup(m => m.IsTheCurrentGame).Returns(true);
            gameState.Setup(m => m.LeagueId).Returns(currentLeagueId);
            gameState.Setup(m => m.AddRegion(givenRegion)).Verifiable();

            sut.AddRegion(currentLeagueId, givenRegion);

            gameState.Verify();
        }

        [Theory, AutoMoq]
        public void AddRegion_GivenTheGameStateIsNotCurrent_ShouldNotAddRegion(
            [Frozen]Mock<IGameState> gameState,
            Guid currentLeagueId,
            Region givenRegion,
            GameManager sut)
        {
            gameState.Setup(m => m.LeagueId).Returns(currentLeagueId);
            gameState.Setup(m => m.IsTheCurrentGame).Returns(false);

            try
            {
                sut.AddRegion(currentLeagueId, givenRegion);
            }
            catch(InvalidOperationException)
            {}

            gameState.Verify(m => m.AddRegion(It.IsAny<Region>()), Times.Never);
        }

        [Theory, AutoMoq]
        public void AddRegion_GivenTheGameStateIsNotCurrent_ShouldThrowInvalidOperationException(
            [Frozen]Mock<IGameState> gameState,
            Guid currentLeagueId,
            Region givenRegion,
            GameManager sut)
        {
            gameState.Setup(m => m.IsTheCurrentGame).Returns(false);
            gameState.Setup(m => m.LeagueId).Returns(currentLeagueId);

            Action action = () => sut.AddRegion(currentLeagueId, givenRegion);

            action.ShouldThrow<InvalidOperationException>();
        }

        [Theory, AutoMoq]
        public void AddRegion_GivenAnInvalidLeagueId_ShouldNotAddRegion(
            [Frozen]Mock<IGameState> gameState,
            Guid currentLeagueId,
            Guid givenLeagueId,
            Region givenRegion,
            GameManager sut)
        {
            gameState.Setup(m => m.IsTheCurrentGame).Returns(true);
            gameState.Setup(m => m.LeagueId).Returns(currentLeagueId);

            try
            {
                sut.AddRegion(givenLeagueId, givenRegion);
            }
            catch(ArgumentException)
            { }

            gameState.Verify(m => m.AddRegion(It.IsAny<Region>()), Times.Never);
        }

        [Theory, AutoMoq]
        public void AddRegion_GivenAnInvalidLeagueId_ShouldThrowArgumentException(
            [Frozen]Mock<IGameState> gameState,
            Guid currentLeagueId,
            Guid givenLeagueId,
            Region givenRegion,
            GameManager sut)
        {
            gameState.Setup(m => m.IsTheCurrentGame).Returns(true);
            gameState.Setup(m => m.LeagueId).Returns(currentLeagueId);

            Action action = () => sut.AddRegion(givenLeagueId, givenRegion);

            action.ShouldThrow<ArgumentException>();
        }

        [Theory, AutoMoq]
        public void BuildBlock_GivenTheGameStateIsCurrent_ShouldBuildBlock(
            [Frozen]Mock<IGameState> gameState,
            Guid leagueId,
            string regionName,
            GameManager sut)
        {
            gameState.Setup(m => m.IsTheCurrentGame).Returns(true);
            gameState.Setup(m => m.LeagueId).Returns(leagueId);
            gameState.Setup(m => m.BuildBlock(regionName)).Verifiable();

            sut.BuildBlock(leagueId, regionName);

            gameState.Verify();
        }

        [Theory, AutoMoq]
        public void BuildBlock_GivenTheGameStateIsNotCurrent_ShouldNotBuildBlock(
            [Frozen]Mock<IGameState> gameState,
            Guid leagueId,
            string regionName,
            GameManager sut)
        {
            gameState.Setup(m => m.IsTheCurrentGame).Returns(false);
            gameState.Setup(m => m.LeagueId).Returns(leagueId);

            try
            {
                sut.BuildBlock(leagueId, regionName);
            }
            catch(InvalidOperationException)
            { }

            gameState.Verify(m => m.BuildBlock(regionName), Times.Never);
        }

        [Theory, AutoMoq]
        public void BuildBlock_GivenTheGameStateIsNotCurrent_ShouldThrowInvalidOperationException(
            [Frozen]Mock<IGameState> gameState,
            Guid leagueId,
            string regionName,
            GameManager sut)
        {
            gameState.Setup(m => m.IsTheCurrentGame).Returns(false);
            gameState.Setup(m => m.LeagueId).Returns(leagueId);

            Action action = () => sut.BuildBlock(leagueId, regionName);

            action.ShouldThrow<InvalidOperationException>();
        }

        [Theory, AutoMoq]
        public void BuildBlock_GivenTheWrongLeagueId_ShouldNotBuildBlock(
            [Frozen]Mock<IGameState> gameState,
            Guid currentLeagueId,
            Guid givenLeagueId,
            string regionName,
            GameManager sut)
        {
            gameState.Setup(m => m.IsTheCurrentGame).Returns(true);
            gameState.Setup(m => m.LeagueId).Returns(currentLeagueId);

            try
            {
                sut.BuildBlock(givenLeagueId, regionName);
            }
            catch(ArgumentException)
            { }

            gameState.Verify(m => m.BuildBlock(regionName), Times.Never);
        }

        [Theory, AutoMoq]
        public void BuildBlock_GivenTheWrongLeagueId_ShouldThrowArgumentException(
            [Frozen]Mock<IGameState> gameState,
            Guid currentLeagueId,
            Guid givenLeagueId,
            string regionName,
            GameManager sut)
        {
            gameState.Setup(m => m.IsTheCurrentGame).Returns(true);
            gameState.Setup(m => m.LeagueId).Returns(currentLeagueId);

            Action action = () => sut.BuildBlock(givenLeagueId, regionName);

            action.ShouldThrow<ArgumentException>();
        }
    }
}
