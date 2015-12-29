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
        public async Task GetAll_ShouldReturnListOfRealms(
            [Frozen] Mock<IGetRealms> getRealmsService, 
            RealmsController sut,
            ICollection<Realm> realms)
        {
            getRealmsService.Setup(m => m.GetRealmsAsync()).Returns(Task.FromResult(realms));

            var actual = await sut.GetRealms();

            actual.Should().BeOfType<HttpOkObjectResult>();
            actual.As<HttpOkObjectResult>().Value.Should().Be(realms);
        }
    }
}
