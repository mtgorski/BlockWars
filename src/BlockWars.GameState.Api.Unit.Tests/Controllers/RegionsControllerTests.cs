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
    public class RegionsControllerTests
    {
        [Theory, AutoMoqController]
        public async Task GetRegionsAsync_ShouldReturnListOfRegions(
            [Frozen] Mock<IGetRegions> getRegionsService,
            ICollection<Region> regions,
            Guid givenLeagueId,
            RegionsController sut)
        {
            getRegionsService.Setup(m => m.GetRegionsAsync(givenLeagueId)).ReturnsAsync(regions);

            var actual = await sut.GetRegionsAsync(givenLeagueId);

            actual.Should().BeAssignableTo<HttpOkObjectResult>();
            actual.As<HttpOkObjectResult>().Value.As<RegionsResponse>().Regions.Should().BeSameAs(regions);
        }

        [Theory, AutoMoqController]
        public async Task PutRegionAsync_ShouldUpsertRegion(
            [Frozen] Mock<IUpsertRegion> upsertRegionService,
            Guid givenLeagueId,
            Guid givenRegionId,
            Region givenRegion,
            RegionsController sut)
        {
            upsertRegionService.Setup(m => m.UpsertRegionAsync(givenLeagueId, givenRegionId, givenRegion))
                .Returns(Task.FromResult(0)).Verifiable();

            var actual = await sut.PutRegionAsync(givenLeagueId, givenRegionId, givenRegion);

            actual.Should().BeAssignableTo<HttpOkResult>();
            upsertRegionService.Verify();
        }

    }
}
