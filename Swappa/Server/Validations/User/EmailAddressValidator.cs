using FluentValidation;

namespace Swappa.Server.Validations.User
{
    public class EmailAddressValidator : AbstractValidator<string>
    {
        public EmailAddressValidator()
        {
            RuleFor(x => x)
                .NotEmpty().WithMessage("Email field is required.")
                .EmailAddress().WithMessage("Please enter a valid e-mail address.");
        }
    }
}
