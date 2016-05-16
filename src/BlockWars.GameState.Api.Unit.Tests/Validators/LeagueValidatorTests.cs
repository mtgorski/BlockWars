using BlockWars.GameState.Api.Validators;
using BlockWars.GameState.Models;
using FluentAssertions;
using System;
using Xunit;

namespace BlockWars.GameState.Api.Unit.Tests.Validators
{
    public class LeagueValidatorTests
    {
        [Theory, AutoMoq]
        public void Validate_GivenLeagueIdIsDefault_ShouldReturnError(
            League league,
            LeagueValidator sut)
        {
            league.LeagueId = Guid.Empty;

            var actual = sut.Validate(league);

            actual.IsValid.Should().BeFalse();
            actual.Errors.Should().ContainSingle(x => x.PropertyName == "LeagueId" && x.ErrorMessage == "LeagueId cannot be default.");
        }

        [Theory]
        [InlineAndAutoMoq(null)]
        [InlineAndAutoMoq("")]
        [InlineAndAutoMoq("  ")]
        public void Validate_GivenNameIsEmpty_ShouldReturnError(
            string leagueName,
            League league,
            LeagueValidator sut)
        {
            league.Name = leagueName;

            var actual = sut.Validate(league);

            actual.IsValid.Should().BeFalse();
            actual.Errors.Should().ContainSingle(x => x.PropertyName == "Name" && x.ErrorMessage == "Name cannot be null or whitespace.");
        }

        [Theory]
        [InlineAndAutoMoq(null)]
        [InlineAndAutoMoq("")]
        [InlineAndAutoMoq("   ")]
        public void Validate_GivenDescriptionIsNullOrWhitespace_ShouldReturnError(
            string leagueDescription,
            League league,
            LeagueValidator sut)
        {
            league.Description = leagueDescription;

            var actual = sut.Validate(league);

            actual.IsValid.Should().BeFalse();
            actual.Errors.Should().ContainSingle(x => x.PropertyName == "Description" && x.ErrorMessage == "Description cannot be null or whitespace.");
        }

        [Theory, AutoMoq]
        public void Validate_GivenDefaultExpiresAt_ShouldReturnError(
            League league, 
            LeagueValidator sut)
        {
            league.ExpiresAt = DateTime.MinValue;

            var actual = sut.Validate(league);

            actual.IsValid.Should().BeFalse();
            actual.Errors.Should().ContainSingle(x => x.PropertyName == "ExpiresAt" && x.ErrorMessage == "ExpiresAt cannot be default.");
        }

        [Theory, AutoMoq]
        public void Validate_GivenNullLeague_ShouldReturnError(
            LeagueValidator sut)
        {
            var actual = sut.Validate(instance:null);

            actual.IsValid.Should().BeFalse();
        }
    }
}
