using FluentValidation;
using Swappa.Shared.DTOs;

namespace Swappa.Server.Validations.Faq
{
    public class FaqValidator : AbstractValidator<BaseFaqDto>
    {
        public FaqValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("{PropertyName} field is required.");
            RuleFor(x => x.Details)
                .NotEmpty().WithMessage("{PropertyName} field is required.");
        }
    }
}
