using BlockWars.GameState.Models;
using FluentValidation;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Filters;
using System;

namespace BlockWars.GameState.Api.Filters
{
    // At the time of coding there's no automatic integration between MVC6 and FluentValidation. Hence this thing.
    public class ValidateLeagueFilter : ActionFilterAttribute
    {
        private readonly AbstractValidator<League> _validator;

        public ValidateLeagueFilter(AbstractValidator<League> validator)
        {
            _validator = validator;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            object leagueIdArgument;
            context.ActionArguments.TryGetValue("leagueId", out leagueIdArgument);
            var leagueId = (Guid)leagueIdArgument;

            object leagueArgument;
            context.ActionArguments.TryGetValue("league", out leagueArgument);
            var league = (League)leagueArgument;

            league.LeagueId = leagueId;

            var validationResult = _validator.Validate(league);
            
            if(!validationResult.IsValid)
            {
                context.Result = new BadRequestObjectResult(validationResult.Errors);
            }
        }
    }
    
}
