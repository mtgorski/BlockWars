using BlockWars.GameState.Api.Controllers;
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
    public class RealmsControllerTests
    {
        [Theory, AutoMoqController]
        public async Task GetRealmsAsync_ShouldReturnListOfRealms(
            [Frozen] Mock<IGetRealms> getRealmsService, 
            RealmsController sut,
            ICollection<Realm> realms)
        {
            getRealmsService.Setup(m => m.GetRealmsAsync()).Returns(Task.FromResult(realms));

            var actual = await sut.GetRealmsAsync();

            actual.Should().BeOfType<HttpOkObjectResult>();
            actual.As<HttpOkObjectResult>().Value.Should().Be(realms);
        }

        [Theory, AutoMoqController]
        public async Task PutRealmAsync_ShouldReturnOk(
            [Frozen] Mock<IUpsertRealm> upsertRealmService,
            RealmsController sut,
            string givenRealmId,
            Realm givenRealm)
        {
            upsertRealmService.Setup(m => m.UpsertRealmAsync(givenRealmId, givenRealm)).Returns(Task.FromResult(0)).Verifiable();

            var actual = await sut.PutRealmAsync(givenRealmId, givenRealm);

            actual.Should().BeOfType<HttpOkResult>();
            upsertRealmService.Verify();
        }

        [Theory, AutoMoqController]
        public async Task PutRealmAsync_GivenModelErrors_ShouldReturn400(
            [Frozen] Mock<IUpsertRealm> upsertRealmService,
            RealmsController sut,
            string errorKey,
            string errorMessage,
            string givenRealmId,
            Realm givenRealm)
        {
            sut.ModelState.AddModelError(errorKey, errorMessage);

            var actual = await sut.PutRealmAsync(givenRealmId, givenRealm);

            actual.Should().BeOfType<BadRequestObjectResult>();
            //TODO: test that the correct model state is returned
            upsertRealmService.Verify(m => m.UpsertRealmAsync(It.IsAny<string>(), It.IsAny<Realm>()), Times.Never);
        }
    }
}
