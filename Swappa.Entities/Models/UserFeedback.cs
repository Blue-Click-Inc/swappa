using Mongo.Common;
using MongoDbGenericRepository.Attributes;
using Swappa.Entities.Enums;

namespace Swappa.Entities.Models
{
    [CollectionName("UserFeedbacks")]
    public class UserFeedback : IBaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public bool IsDeprecated { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string UserEmail { get; set; } = string.Empty;
        public string Feedback { get; set; } = string.Empty;
        public FeedbackRating Rating { get; set; } = FeedbackRating.Excellent;
    }
}
