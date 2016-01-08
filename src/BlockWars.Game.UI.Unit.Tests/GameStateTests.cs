using BlockWars.GameState.Models;
using FluentAssertions;
using FluentAssertions.Equivalency;
using Ploeh.AutoFixture.Xunit2;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace BlockWars.Game.UI.Unit.Tests
{
    public class GameStateTests
    {
        [Theory, AutoMoq]
        public void AddRegion_ShouldAddTheRegion(
            Region givenRegion,
            GameState sut)
        {
            sut.AddRegion(givenRegion);

            (sut.Regions[givenRegion.Name] == givenRegion).Should().BeTrue();
        }

        [Theory, AutoMoq]
        public void AddRegion_GivenThereIsAlreadyARegionWithTheSameName_ShouldThrowInvalidRegionException(
            Region existingRegion,
            Region newRegion,
            GameState sut)
        {
            sut.AddRegion(existingRegion);
            newRegion.Name = existingRegion.Name;

            Action sutAction = () => sut.AddRegion(newRegion);

            sutAction.ShouldThrow<GameState.InvalidRegionException>();
        }

        [Theory, AutoMoq]
        public void BuildBlock_GivenAValidRegion_ShouldIncrementBlockCount(
            Region region,
            GameState sut)
        {
            sut.AddRegion(region);
            var initialCount = region.BlockCount;

            sut.BuildBlock(region.Name);

            sut.Regions[region.Name].BlockCount.Should().Be(initialCount + 1);
        }

        [Theory, AutoMoq]
        public void ToView_ContainsTheLeagueInfo(
            [Frozen] League league,
            GameState sut)
        {
            var view = sut.ToView();

            Func<EquivalencyAssertionOptions<League>,
                EquivalencyAssertionOptions<League>> comparisonOptions
                 = o => o.IncludingAllDeclaredProperties();
            view.League.ShouldBeEquivalentTo(league, comparisonOptions); 
        }

        [Theory, AutoMoq]
        public void ToView_ContainsTheRegionsThatHaveBeenAdded(
            List<Region> regions,
            GameState sut)
        {
            regions.ForEach(x => sut.AddRegion(x));

            var view = sut.ToView();

            view.Regions.Should().HaveCount(regions.Count);
            Func<EquivalencyAssertionOptions<Region>,
                EquivalencyAssertionOptions<Region>> comparisonOptions
                = o => o.IncludingAllDeclaredProperties();
            foreach(var region in regions)
            {
                var expected = view.Regions.Single(x => x.RegionId == region.RegionId);
                expected.ShouldBeEquivalentTo(region, comparisonOptions);
            }
        }
        
    }
}
