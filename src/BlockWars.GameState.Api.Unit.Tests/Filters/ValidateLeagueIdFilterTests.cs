using BlockWars.GameState.Api.Filters;
using BlockWars.GameState.Api.HttpUtility;
using BlockWars.GameState.Api.Validators.Interfaces;
using FluentAssertions;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Filters;
using Moq;
using Ploeh.AutoFixture.Xunit2;
using Ploeh.SemanticComparison.Fluent;
using System;
using System.Threading.Tasks;
using Xunit;

namespace BlockWars.GameState.Api.Unit.Tests.Attributes
{
    public class ValidateLeagueIdFilterTests
    {

        [Theory, AutoMoq]
        public async Task OnActionExecution_GivenValidId_ShouldNotSetResult(
            [Frozen] Mock<IValidateLeagueId> validator,
            ValidateLeagueIdFilter sut,
            ActionExecutingContext givenContext,
            ActionExecutedContext executedContext,
            Guid givenLeagueId)
        {
            givenContext.ActionArguments["leagueId"] = givenLeagueId;
            givenContext.Result = null;
            validator.Setup(m => m.ValidateLeagueIdAsync(givenLeagueId)).ReturnsAsync(true);

            await sut.OnActionExecutionAsync(givenContext, () => Task.FromResult(executedContext));

            givenContext.Result.Should().BeNull();
        }

        public interface IMockNextActionFilter
        {
            Task<ActionExecutedContext> InvokeAsync();
        }

        [Theory, AutoMoq]
        public async Task OnActionExecution_GivenValidId_ShouldInvokeNext(
            [Frozen] Mock<IValidateLeagueId> validator,
            [Frozen] Mock<IMockNextActionFilter> nextFilter,
            ValidateLeagueIdFilter sut,
            ActionExecutingContext givenContext,
            ActionExecutedContext executedContext,
            Guid givenLeagueId)
        {
            givenContext.ActionArguments["leagueId"] = givenLeagueId;
            givenContext.Result = null;
            validator.Setup(m => m.ValidateLeagueIdAsync(givenLeagueId)).ReturnsAsync(true);

            await sut.OnActionExecutionAsync(givenContext, nextFilter.Object.InvokeAsync);

            nextFilter.Verify(m => m.InvokeAsync(), Times.Once);
        }

        [Theory, AutoMoq]
        public async Task OnActionExecution_GivenInvalidId_ShouldNotInvokeNext(
            [Frozen] Mock<IValidateLeagueId> validator,
            [Frozen] Mock<IMockNextActionFilter> nextFilter,
            ValidateLeagueIdFilter sut,
            ActionExecutingContext givenContext,
            ActionExecutedContext executedContext,
            Guid givenLeagueId)
        {
            givenContext.ActionArguments["leagueId"] = givenLeagueId;
            givenContext.Result = null;
            validator.Setup(m => m.ValidateLeagueIdAsync(givenLeagueId)).ReturnsAsync(false);

            await sut.OnActionExecutionAsync(givenContext, nextFilter.Object.InvokeAsync);

            nextFilter.Verify(m => m.InvokeAsync(), Times.Never);
        }

        [Theory, AutoMoq]
        public async Task OnActionExecution_GivenInValidId_ShouldReturn404(
            [Frozen] Mock<IValidateLeagueId> validator,
            [Frozen] Mock<IMockNextActionFilter> nextFilter,
            ValidateLeagueIdFilter sut,
            ActionExecutingContext givenContext,
            ActionExecutedContext executedContext,
            Guid givenLeagueId)
        {
            givenContext.ActionArguments["leagueId"] = givenLeagueId;
            givenContext.Result = null;
            validator.Setup(m => m.ValidateLeagueIdAsync(givenLeagueId)).ReturnsAsync(false);

            await sut.OnActionExecutionAsync(givenContext, nextFilter.Object.InvokeAsync);

            var result = givenContext.Result;

            result.Should().BeAssignableTo<HttpNotFoundObjectResult>();
            var value = (NotFoundValue)((HttpNotFoundObjectResult)result).Value;
            var expectedValue = new NotFoundValue($"{givenLeagueId} is not a valid LeagueId.")
                .AsSource().OfLikeness<NotFoundValue>();
            expectedValue.ShouldEqual(value);
        }
    }
}
