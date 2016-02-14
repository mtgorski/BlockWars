using BlockWars.GameState.Models;
using FluentValidation;

namespace BlockWars.GameState.Api.Validators
{
    public class RegionValidator : AbstractValidator<Region>
    {
        public RegionValidator()
        {
            RuleFor(x => x.RegionId).NotEmpty().WithMessage("RegionId cannot be default.");
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name must be non-empty.");
        }
    }
}
