using Swappa.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace Swappa.Shared.DTOs
{
    public record FeedbackForAddDto
    {
        [Required(ErrorMessage = "Email field is required"), EmailAddress]
        public string UserEmail { get; set; } = string.Empty;
        [Required(ErrorMessage = "Feedback field is required.")]
        public string Feedback { get; set; } = string.Empty;
        public FeedbackRating Rating { get; set; }
    }
}
