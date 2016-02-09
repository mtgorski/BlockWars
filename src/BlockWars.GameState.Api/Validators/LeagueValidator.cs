using BlockWars.GameState.Models;
using FluentValidation;

namespace BlockWars.GameState.Api.Validators
{
    public class LeagueValidator : AbstractValidator<League>
    {
        public LeagueValidator()
        {
            RuleFor(x => x.LeagueId).NotEmpty().WithMessage("LeagueId cannot be default.");
            RuleFor(x => x.Name).NotNull().Must(NotBeWhitespace).WithMessage("Name cannot be null or whitespace.");
            RuleFor(x => x.Description).NotNull().Must(NotBeWhitespace).WithMessage("Description cannot be null or whitespace.");
            RuleFor(x => x.ExpiresAt).NotEmpty().WithMessage("ExpiresAt cannot be default.");
        }

        private bool NotBeWhitespace(string str)
        {
            return !string.IsNullOrWhiteSpace(str);
        }
    }
}
