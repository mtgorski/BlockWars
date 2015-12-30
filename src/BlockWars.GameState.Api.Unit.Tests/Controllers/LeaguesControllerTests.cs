﻿using BlockWars.GameState.Api.Controllers;
using BlockWars.GameState.Models;
using FluentAssertions;
using Microsoft.AspNet.Mvc;
using Moq;
using Ploeh.AutoFixture.Xunit2;
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
            LeaguesController sut,
            ICollection<League> leagues)
        {
            getLeaguesService.Setup(m => m.GetLeaguesAsync()).Returns(Task.FromResult(leagues));

            var actual = await sut.GetLeaguesAsync();

            actual.Should().BeOfType<HttpOkObjectResult>();
            actual.As<HttpOkObjectResult>().Value.Should().Be(leagues);
        }

        [Theory, AutoMoqController]
        public async Task PutLeagueAsync_ShouldReturnOk(
            [Frozen] Mock<IUpsertLeague> upsertLeagueService,
            LeaguesController sut,
            string givenLeagueId,
            League givenLeague)
        {
            upsertLeagueService.Setup(m => m.UpsertLeagueAsync(givenLeagueId, givenLeague)).Returns(Task.FromResult(0)).Verifiable();

            var actual = await sut.PutLeagueAsync(givenLeagueId, givenLeague);

            actual.Should().BeOfType<HttpOkResult>();
            upsertLeagueService.Verify();
        }

        [Theory, AutoMoqController]
        public async Task PutLeagueAsync_GivenModelErrors_ShouldReturn400(
            [Frozen] Mock<IUpsertLeague> upsertLeagueService,
            LeaguesController sut,
            string errorKey,
            string errorMessage,
            string givenLeagueId,
            League givenLeague)
        {
            sut.ModelState.AddModelError(errorKey, errorMessage);

            var actual = await sut.PutLeagueAsync(givenLeagueId, givenLeague);

            actual.Should().BeOfType<BadRequestObjectResult>();
            //TODO: test that the correct model state is returned
            upsertLeagueService.Verify(m => m.UpsertLeagueAsync(It.IsAny<string>(), It.IsAny<League>()), Times.Never);
        }
    }
}
