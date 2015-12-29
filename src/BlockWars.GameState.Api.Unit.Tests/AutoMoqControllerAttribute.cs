using Microsoft.AspNet.Mvc;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Xunit2;
using System.Threading.Tasks;
using System;
using Ploeh.AutoFixture.Kernel;
using Ploeh.AutoFixture.AutoMoq;

namespace BlockWars.GameState.Api.Unit.Tests
{
    public class AutoMoqControllerAttribute : AutoDataAttribute
    {
        public AutoMoqControllerAttribute() :
            base(new Fixture()
                    .Customize(new AutoMoqCustomization())
                    .Customize(new ControllerCustomization()))
        {

        }

        public class ControllerCustomization : ICustomization
        {
            public void Customize(IFixture fixture)
            {
                fixture.Customize<Controller>(x => x.Without(y => y.ViewData));
            }
        }
    }
}
