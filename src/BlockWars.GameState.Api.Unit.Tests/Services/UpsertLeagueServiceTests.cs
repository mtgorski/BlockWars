using AutoMapper;
using BlockWars.GameState.Api.DataModels;
using BlockWars.GameState.Api.Repositories;
using BlockWars.GameState.Api.Services;
using BlockWars.GameState.Models;
using Moq;
using Ploeh.AutoFixture.Xunit2;
using System;
using System.Threading.Tasks;
using Xunit;

namespace BlockWars.GameState.Api.Unit.Tests.Services
{
    public class UpsertLeagueServiceTests
    {
        [Theory, AutoMoq]
        public async Task UpsertLeagueAsync_ShouldUpsertLeague(
            [Frozen] Mock<ILeagueRepository> leagueRepo,
            [Frozen] Mock<IMappingEngine> mapper,
            Guid givenLeagueId,
            League givenLeague,
            LeagueData leagueData,
            UpsertLeagueService sut)
        {
            mapper.Setup(m => m.Map<LeagueData>(givenLeague)).Returns(leagueData);
            leagueRepo.Setup(
                m => m.UpsertLeagueAsync(
                    givenLeagueId,
                    It.Is<LeagueData>(x => x.LeagueId == givenLeagueId && x == leagueData)))
                .Returns(Task.FromResult(0)).Verifiable();

            await sut.UpsertLeagueAsync(givenLeagueId, givenLeague);

            leagueRepo.Verify();
        } 


    }
}
