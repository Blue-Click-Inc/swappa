using Mongo.Common;
using MongoDbGenericRepository.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Swappa.Entities.Models
{
    [CollectionName("UserFeedbacks")]
    public class ContactMessage : IBaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required]
        public string Message { get; set; } = string.Empty;
        [Required]
        public string EmailAddress { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public bool IsDeprecated { get; set; }
        public bool IsRead { get; set; }
    }
}
