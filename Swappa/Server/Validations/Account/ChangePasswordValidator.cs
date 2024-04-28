using FluentValidation;
using Swappa.Server.Commands.Account;

namespace Swappa.Server.Validations.Account
{
    public class ChangePasswordValidator : AbstractValidator<ChangePasswordCommand>
    {
        public ChangePasswordValidator()
        {
            RuleFor(x => x.Request.NewPassword)
                .NotEmpty().WithMessage("{PropertyName} field is required.")
                .MinimumLength(8).WithMessage("{PropertyName} field must be at least 6 characters.");
            RuleFor(x => x.Request.CurrentPassword)
                .NotEmpty().WithMessage("{PropertyName} field is required.")
                .MinimumLength(8).WithMessage("{PropertyName} field must be at least 6 characters.");
        }
    }
}
