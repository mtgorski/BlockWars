using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;
using Ploeh.AutoFixture.Xunit2;

namespace BlockWars.Game.UI.Unit.Tests
{
    public class AutoMoqAttribute : AutoDataAttribute
    {
        public AutoMoqAttribute() :
            base(new Fixture()
                    .Customize(new AutoMoqCustomization()))
        {
        }
    }
}
