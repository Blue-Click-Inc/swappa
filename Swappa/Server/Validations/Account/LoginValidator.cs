using FluentValidation;
using Swappa.Server.Commands.Account;

namespace Swappa.Server.Validations.Account
{
    public class LoginValidator : AbstractValidator<LoginCommand>
    {
        public LoginValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("{PropertyName} field is required.")
                .EmailAddress().WithMessage("Please enter a valid email address.");
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("{PropertyName} field is required.")
                .MinimumLength(6).WithMessage("{PropertyName} field must be at least 6 characters.");
        }
    }
}
