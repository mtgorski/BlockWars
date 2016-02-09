using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;
using Ploeh.AutoFixture.Xunit2;

namespace BlockWars.GameState.Api.Unit.Tests
{
    public class AutoMoqAttribute : AutoDataAttribute
    {
        public AutoMoqAttribute() :
            base(new Fixture()
                    .Customize(new AutoMoqCustomization()))
        {
        }
    }

    public class InlineAndAutoMoqAttribute : InlineAutoDataAttribute
    {
        public InlineAndAutoMoqAttribute(params object[] values) : base(new AutoMoqAttribute(), values)
        {
        }
    }
}
