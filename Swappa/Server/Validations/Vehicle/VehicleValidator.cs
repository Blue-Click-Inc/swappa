using FluentValidation;
using Swappa.Shared.DTOs;

namespace Swappa.Server.Validations.Vehicle
{
    public class VehicleValidator : AbstractValidator<BaseVehicleDto>
    {
        public VehicleValidator()
        {
            RuleFor(x => x.Model)
                .NotEmpty().WithMessage("{PropertyName} field is required.")
            RuleFor(x => x.Make)
                .NotEmpty().WithMessage("{PropertyName} field is required.");
            RuleFor(x => x.Price)
                .NotEmpty().WithMessage("{PropertyName} field must be greater than 0.");
            RuleFor(x => x.Year)
                .NotEmpty().WithMessage("{PropertyName} field must be greater than 0.");
            RuleFor(x => x.Transmission)
                .IsInEnum().WithMessage("Please enter a valid {PropertyName}");
            RuleFor(x => x.DriveTrain)
                .IsInEnum().WithMessage("Please enter a valid Drive Train");
            RuleFor(x => x.Engine)
                .IsInEnum().WithMessage("Please enter a valid {PropertyName}");
            RuleFor(x => x.Color)
                .IsInEnum().WithMessage("{PropertyName} field is required.");
        }
    }
}
