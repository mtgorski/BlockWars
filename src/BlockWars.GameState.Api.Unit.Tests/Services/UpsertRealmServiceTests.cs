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
    public class UpsertRealmServiceTests
    {
        [Theory, AutoMoq]
        public async Task UpsertRealmAsync_ShouldUpsertRealm(
            [Frozen] Mock<IRealmRepository> realmRepo,
            [Frozen] Mock<IMappingEngine> mapper,
            string givenRealmId,
            Realm givenRealm,
            RealmData realmData,
            UpsertRealmService sut)
        {
            mapper.Setup(m => m.Map<RealmData>(givenRealm)).Returns(realmData);
            realmRepo.Setup(
                m => m.UpsertRealmAsync(
                    givenRealmId,
                    It.Is<RealmData>(x => x.RealmId == givenRealmId && x == realmData)))
                .Returns(Task.FromResult(0)).Verifiable();

            await sut.UpsertRealmAsync(givenRealmId, givenRealm);

            realmRepo.Verify();
        } 


    }
}
