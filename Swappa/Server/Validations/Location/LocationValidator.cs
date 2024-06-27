using FluentValidation;
using Swappa.Shared.DTOs;

namespace Swappa.Server.Validations.Location
{
    public class LocationValidator : AbstractValidator<BaseLocationDto>
    {
        public LocationValidator()
        {
            RuleFor(x => x.CountryId)
                .NotEmpty().WithMessage("{PropertyName} field is required.")
                .MinimumLength(24).WithMessage("{PropertyName} field must be at least 24 characters.");
            RuleFor(x => x.StateId)
                .NotEmpty().WithMessage("{PropertyName} field is required.")
                .MinimumLength(24).WithMessage("{PropertyName} field must be at least 24 characters.");
            RuleFor(x => x.City)
                .NotEmpty().WithMessage("{PropertyName} field is required.");
            RuleFor(x => x.PostalCode)
                .NotEmpty().WithMessage("{PropertyName} field is required.")
                .MinimumLength(5).WithMessage("{PropertyName} field must be at least 5 characters.");
            RuleFor(x => x.EntityId)
                .NotEmpty().WithMessage("{PropertyName} field is required.");
            RuleFor(x => x.EntityType)
                .IsInEnum().WithMessage("Please enter a valid entity type");
        }
    }
}
