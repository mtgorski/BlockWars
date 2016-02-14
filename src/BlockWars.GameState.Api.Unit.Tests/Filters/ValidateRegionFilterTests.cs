using BlockWars.GameState.Api.Filters;
using BlockWars.GameState.Models;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Filters;
using Moq;
using Ploeh.AutoFixture.Xunit2;
using System;
using Xunit;

namespace BlockWars.GameState.Api.Unit.Tests.Filters
{
    public class ValidateRegionFilterTests
    {
        [Theory, AutoMoq]
        public void OnActionExecution_GivenValidRegion_ShouldNotSetResult(
            [Frozen] Mock<AbstractValidator<Region>> validator,
            ValidateRegionFilter sut,
            ActionExecutingContext givenContext,
            Guid givenRegionId,
            Region givenRegion)
        {
            givenContext.ActionArguments["regionId"] = givenRegionId;
            givenContext.ActionArguments["region"] = givenRegion;
            var validationResult = new ValidationResult(new ValidationFailure[0]);
            validator.Setup(m => m.Validate(It.Is<Region>(x => givenRegion == x && x.RegionId == givenRegionId)))
                .Returns(validationResult);
            givenContext.Result = null;

            sut.OnActionExecuting(givenContext);

            givenContext.Result.Should().BeNull();
        }

        [Theory, AutoMoq]
        public void OnActionExecution_GivenInvalidRegion_ShouldReturnBadRequestResult(
            [Frozen] Mock<AbstractValidator<Region>> validator,
            ValidateRegionFilter sut,
            ActionExecutingContext givenContext,
            Guid givenRegionId,
            Region givenRegion)
        {
            givenContext.ActionArguments["regionId"] = givenRegionId;
            givenContext.ActionArguments["region"] = givenRegion;
            var validationResult = new ValidationResult(new ValidationFailure[] { new ValidationFailure("Name", "error") });
            validator.Setup(m => m.Validate(It.Is<Region>(x => givenRegion == x && x.RegionId == givenRegionId)))
                .Returns(validationResult);
            givenContext.Result = null;

            sut.OnActionExecuting(givenContext);

            givenContext.Result.Should().NotBeNull();
            givenContext.Result.Should().BeAssignableTo<BadRequestObjectResult>();
            givenContext.Result.As<BadRequestObjectResult>().Value.Should().Be(validationResult);
        }
    }
}
