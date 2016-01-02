using AutoMapper;
using BlockWars.GameState.Api.DataModels;
using BlockWars.GameState.Api.Queries;
using BlockWars.GameState.Api.Repositories;
using BlockWars.GameState.Api.Services;
using BlockWars.GameState.Models;
using FluentAssertions;
using Moq;
using Ploeh.AutoFixture.Xunit2;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace BlockWars.GameState.Api.Unit.Tests.Services
{
    public class GetLeaguesServiceTests
    {
        [Theory, AutoMoq]
        public async Task GetLeaguesAsync_ShouldReturnListOfLeagues(
            [Frozen]Mock<ILeagueRepository> leagueRepo,
            [Frozen]Mock<IMappingEngine> mappingEngine,
            LeagueSearchRequest givenSearchRequest,
            List<LeagueData> leagueData,
            List<League> expectedLeagues,
            GetLeaguesService sut)
        {
            leagueRepo.Setup(m => m.GetLeaguesAsync(It.Is<LeagueQuery>(x => x.IsCurrent == givenSearchRequest.IsCurrent))).ReturnsAsync(leagueData);
            mappingEngine.Setup(m => m.Map<List<League>>(leagueData)).Returns(expectedLeagues);

            var actual = await sut.GetLeaguesAsync(givenSearchRequest);

            actual.Should().BeSameAs(expectedLeagues);
        }
    }
}
