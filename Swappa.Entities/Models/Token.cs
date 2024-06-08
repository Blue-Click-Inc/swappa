using Mongo.Common;
using MongoDbGenericRepository.Attributes;
using Swappa.Entities.Enums;

namespace Swappa.Entities.Models
{
    [CollectionName("Tokens")]
    public class Token : IBaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid UserId { get; set; }
        public TokenType Type { get; set; }
        public string Value { get; set; } = string.Empty;
        public DateTime ExpiresAt => CreatedAt.AddDays(1);
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public bool IsDeprecated { get; set; }
    }
}
