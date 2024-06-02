using FluentValidation;
using Swappa.Server.Commands.Account;

namespace Swappa.Server.Validations.Account
{
    public class RegisterValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("{PropertyName} field is required.")
                .EmailAddress().WithMessage("Please enter a valid email address.");
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("{PropertyName} field is required.")
                .MinimumLength(6).WithMessage("{PropertyName} field must be at least 6 characters.");
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("{PropertyName} field is required.");
            RuleFor(x => x.Gender)
                .IsInEnum().WithMessage("Value entered not a valid gender.");
            RuleFor(x => x.Role)
                .IsInEnum().WithMessage("Please enter a valid user role");
        }
    }
}
