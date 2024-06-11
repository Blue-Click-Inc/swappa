using Swappa.Entities.Enums;

namespace Swappa.Shared.DTOs
{
    public class UserFeedbackDto
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsDeprecated { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string UserEmail { get; set; } = string.Empty;
        public string Feedback { get; set; } = string.Empty;
        public FeedbackRating Rating { get; set; }
    }
}
