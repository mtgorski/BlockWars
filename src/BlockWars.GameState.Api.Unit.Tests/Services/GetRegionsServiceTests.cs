using AutoMapper;
using BlockWars.GameState.Api.DataModels;
using BlockWars.GameState.Api.Repositories;
using BlockWars.GameState.Api.Services;
using BlockWars.GameState.Models;
using FluentAssertions;
using Moq;
using Ploeh.AutoFixture.Xunit2;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace BlockWars.GameState.Api.Unit.Tests.Services
{
    public class GetRegionsServiceTests
    {
        [Theory, AutoMoq]
        public async Task GetRegions_ShouldReturnRegions(
            [Frozen] Mock<IRegionRepository> regionRepository,
            [Frozen] Mock<IMappingEngine> mapper,
            Guid givenLeagueId,
            ICollection<RegionData> regionData,
            List<Region> regions,
            GetRegionsService sut)
        {
            regionRepository.Setup(m => m.GetRegionsAsync(givenLeagueId)).ReturnsAsync(regionData);
            mapper.Setup(m => m.Map<List<Region>>(regionData)).Returns(regions);

            var actual = await sut.GetRegionsAsync(givenLeagueId);

            actual.Should().BeSameAs(regions);
        }
    }
}
