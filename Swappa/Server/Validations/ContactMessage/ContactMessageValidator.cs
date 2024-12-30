using FluentValidation;
using Swappa.Shared.DTOs;

namespace Swappa.Server.Validations.ContactMessage
{
    public class ContactMessageValidator : AbstractValidator<ContactMessageBaseDto>
    {
        public ContactMessageValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("{PropertyName} field is required.");
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("{PropertyName} field is required.");
            RuleFor(x => x.Message)
                .NotEmpty().WithMessage("{PropertyName} field is required.");
        }
    }
}
