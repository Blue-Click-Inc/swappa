using FluentValidation;
using Swappa.Shared.DTOs;

namespace Swappa.Server.Validations.User
{
    public class UpdateDetailsValidator : AbstractValidator<UserDetailsForUpdateDto>
    {
        public UpdateDetailsValidator()
        {
            RuleFor(x => x.Name)
               .NotEmpty().WithMessage("{PropertyName} field is required.");
            RuleFor(x => x.Gender)
                .IsInEnum().WithMessage("Value entered not a valid gender.");
        }
    }
}
