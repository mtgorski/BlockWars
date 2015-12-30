using AutoMapper;
using BlockWars.GameState.Api.DataModels;
using BlockWars.GameState.Api.Repositories;
using BlockWars.GameState.Api.Services;
using BlockWars.GameState.Models;
using Moq;
using Ploeh.AutoFixture.Xunit2;
using System.Threading.Tasks;
using Xunit;

namespace BlockWars.GameState.Api.Unit.Tests.Services
{
    public class UpsertRegionServiceTests
    {
        [Theory, AutoMoq]
        public async Task UpsertRegionAsync_ShouldUpsertRegion(
            [Frozen]Mock<IRegionRepository> regionRepository,
            [Frozen]Mock<IMappingEngine> mappingEngine,
            string givenLeagueId,
            string givenRegionId,
            Region givenRegion,
            RegionData regionData,
            UpsertRegionService sut)
        {
            mappingEngine.Setup(m => m.Map<RegionData>(givenRegion)).Returns(regionData);
            regionRepository.Setup(
                m => m.UpsertRegionAsync(givenRegionId, It.Is<RegionData>(r => r.RegionId == givenRegionId && r.LeagueId == givenLeagueId)))
               .Returns(Task.FromResult(0)).Verifiable();

            await sut.UpsertRegionAsync(givenLeagueId, givenRegionId, givenRegion);

            regionRepository.Verify();
        }
    }
}
