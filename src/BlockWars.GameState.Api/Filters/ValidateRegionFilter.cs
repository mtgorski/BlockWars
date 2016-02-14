using BlockWars.GameState.Models;
using FluentValidation;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Filters;
using System;

namespace BlockWars.GameState.Api.Filters
{
    public class ValidateRegionFilter : ActionFilterAttribute
    {
        private readonly AbstractValidator<Region> _validator;

        public ValidateRegionFilter(AbstractValidator<Region> validator)
        {
            _validator = validator;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var region = (Region)context.ActionArguments["region"];
            var regionId = (Guid)context.ActionArguments["regionId"];
            region.RegionId = regionId;
            var validationResult = _validator.Validate(region);
            if(!validationResult.IsValid)
            {
                context.Result = new BadRequestObjectResult(validationResult);
            }
        }
    }
}
