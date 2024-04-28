using FluentValidation;
using Swappa.Server.Commands.Account;

namespace Swappa.Server.Validations.Account
{
    public class ForgotPasswordValidator : AbstractValidator<ForgotPasswordCommand>
    {
        public ForgotPasswordValidator()
        {
            RuleFor(x => x.Request.NewPassword)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .MinimumLength(8).WithMessage("{PropertyName} length must be at least, 8 characters long.");
            RuleFor(x => x.Request.ConfirmNewPassword)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .MinimumLength(8).WithMessage("{PropertyName} length must be at least, 8 characters long.");
            RuleFor(x => x.Request.Token)
                .NotEmpty().WithMessage("Invalid token");
        }
    }
}
