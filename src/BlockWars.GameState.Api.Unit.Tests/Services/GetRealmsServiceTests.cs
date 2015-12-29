using AutoMapper;
using BlockWars.GameState.Api.DataModels;
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
    public class GetRealmsServiceTests
    {
        [Theory, AutoMoq]
        public async Task GetRealmsAsync_ShouldReturnListOfRealms(
            [Frozen]Mock<IRealmRepository> realmRepo,
            [Frozen]Mock<IMappingEngine> mappingEngine,
            List<RealmData> realmData,
            List<Realm> expectedRealms,
            GetRealmsService sut)
        {
            realmRepo.Setup(m => m.GetRealmsAsync()).ReturnsAsync(realmData);
            mappingEngine.Setup(m => m.Map<List<Realm>>(realmData)).Returns(expectedRealms);

            var actual = await sut.GetRealmsAsync();

            actual.Should().BeSameAs(expectedRealms);
        }
    }
}
