using System.Threading.Tasks;
using Microsoft.AspNet.Mvc.Filters;
using BlockWars.GameState.Api.Validators.Interfaces;
using System;
using Microsoft.AspNet.Mvc;
using BlockWars.GameState.Api.HttpUtility;

namespace BlockWars.GameState.Api.Filters
{
    public class ValidateLeagueIdFilter : ActionFilterAttribute
    {
        private readonly IValidateLeagueId _validator;

        public ValidateLeagueIdFilter(IValidateLeagueId validator)
        {
            _validator = validator;
        }

        public async override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var leagueId = (Guid)context.ActionArguments["leagueId"];

            if(await _validator.ValidateLeagueIdAsync(leagueId))
            {
                await next();
                return;
            }

            context.Result = new HttpNotFoundObjectResult(new NotFoundValue($"{leagueId} is not a valid LeagueId."));
        }
    }
}
