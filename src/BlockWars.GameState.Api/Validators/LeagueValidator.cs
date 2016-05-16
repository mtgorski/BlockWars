using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BlockWars.GameState.Models;
using FluentValidation;
using FluentValidation.Results;
using FluentValidation.Validators;

namespace BlockWars.GameState.Api.Validators
{
    public class LeagueValidator : AbstractValidator<League>
    {
        public LeagueValidator()
        {
            this.CascadeMode = CascadeMode.StopOnFirstFailure;
            RuleFor(x => x.LeagueId).NotEmpty().WithMessage("LeagueId cannot be default.");
            RuleFor(x => x.Name).NotNull().Must(NotBeWhitespace).WithMessage("Name cannot be null or whitespace.");
            RuleFor(x => x.Description).NotNull().Must(NotBeWhitespace).WithMessage("Description cannot be null or whitespace.");
            RuleFor(x => x.ExpiresAt).NotEmpty().WithMessage("ExpiresAt cannot be default.");
        }

        private bool NotBeWhitespace(string str)
        {
            return !string.IsNullOrWhiteSpace(str);
        }

        public override async Task<ValidationResult> ValidateAsync(ValidationContext<League> context, CancellationToken cancellation = default(CancellationToken))
        {
            var instance = context.InstanceToValidate;
            if(instance == null)
            {
                return new ValidationResult(new ValidationFailure[] { new ValidationFailure("quote request", "error") });
            }
            return await base.ValidateAsync(context, cancellation);
        }

        public override ValidationResult Validate(ValidationContext<League> context)
        {
            var instance = context.InstanceToValidate;
            if (instance == null)
            {
                return new ValidationResult(new ValidationFailure[] { new ValidationFailure("quote request", "error") });
            }
            return base.Validate(context);
        }
    }

}
