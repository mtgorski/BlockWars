using BlockWars.GameState.Api.Controllers;
using BlockWars.GameState.Models;
using FluentAssertions;
using Microsoft.AspNet.Mvc;
using Moq;
using Ploeh.AutoFixture.Xunit2;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace BlockWars.GameState.Api.Unit.Tests.Controllers
{
    public class LeaguesControllerTests
    {
        [Theory, AutoMoqController]
        public async Task GetLeaguesAsync_ShouldReturnListOfLeagues(
            [Frozen] Mock<IGetLeagues> getLeaguesService,
            LeagueSearchRequest givenRequest, 
            LeaguesController sut,
            ICollection<League> leagues)
        {
            getLeaguesService.Setup(m => m.GetLeaguesAsync(givenRequest)).Returns(Task.FromResult(leagues));

            var actual = await sut.GetLeaguesAsync(givenRequest);

            actual.Should().BeOfType<HttpOkObjectResult>();
            actual.As<HttpOkObjectResult>().Value.As<LeaguesResponse>().Leagues.Should().BeSameAs(leagues);
        }

        [Theory, AutoMoqController]
        public async Task PutLeagueAsync_ShouldReturnOk(
            [Frozen] Mock<IUpsertLeague> upsertLeagueService,
            LeaguesController sut,
            Guid givenLeagueId,
            League givenLeague)
        {
            upsertLeagueService.Setup(m => m.UpsertLeagueAsync(givenLeagueId, givenLeague)).Returns(Task.FromResult(0)).Verifiable();

            var actual = await sut.PutLeagueAsync(givenLeagueId, givenLeague);

            actual.Should().BeOfType<HttpOkResult>();
            upsertLeagueService.Verify();
        }

    }
}
