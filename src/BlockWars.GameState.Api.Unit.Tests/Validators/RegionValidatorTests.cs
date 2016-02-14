using BlockWars.GameState.Api.Validators;
using BlockWars.GameState.Models;
using FluentAssertions;
using System;
using Xunit;

namespace BlockWars.GameState.Api.Unit.Tests.Validators
{
    public class RegionValidatorTests
    {
        [Theory, AutoMoq]
        public void Validate_GivenEmptyRegionId_ShouldReturnError(
            Region givenRegion,
            RegionValidator sut)
        {
            givenRegion.RegionId = Guid.Empty;

            var result = sut.Validate(givenRegion);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x => x.PropertyName == "RegionId" && x.ErrorMessage == "RegionId cannot be default.");
        }

        [Theory]
        [InlineAndAutoMoq(null)]
        [InlineAndAutoMoq("")]
        [InlineAndAutoMoq(" ")]
        public void Validate_GivenNullOrEmptyName_ShouldReturnError(
            string givenName,
            Region givenRegion,
            RegionValidator sut)
        {
            givenRegion.Name = givenName;

            var result = sut.Validate(givenRegion);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x => x.PropertyName == "Name" && x.ErrorMessage == "Name must be non-empty.");
        }
    }
}
