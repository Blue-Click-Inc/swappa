using AspNetCore.Identity.MongoDbCore.Models;
using Mongo.Common;
using MongoDbGenericRepository.Attributes;
using Swappa.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace Swappa.Entities.Models
{
    [CollectionName("Users")]
    public class AppUser : MongoIdentityUser<Guid>, IBaseEntity
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public Gender Gender { get; set; }
        public Status Status { get; set; } = Status.Inactive;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public bool IsDeprecated { get; set; }
    }
}
