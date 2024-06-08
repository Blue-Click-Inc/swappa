using FluentValidation;
using Swappa.Entities.Enums;
using Swappa.Shared.DTOs;

namespace Swappa.Server.Validations.User
{
    public class SendUserFeedbackValidator : AbstractValidator<FeedbackForAddDto>
    {
        public SendUserFeedbackValidator()
        {
            RuleFor(x => x.UserEmail)
                .NotEmpty().WithMessage("{PropertyName} field is required.")
                .EmailAddress().WithMessage("Please enter a valid email address.");
            RuleFor(x => x.Feedback)
                .NotEmpty().WithMessage("{PropertyName} field is required.");
            RuleFor(x => x.Rating)
                .IsInEnum().WithMessage("Value entered not a valid rating.")
                .NotEqual(FeedbackRating.None).WithMessage("Please select a valid value for {PropertyName}");
        }
    }
}
