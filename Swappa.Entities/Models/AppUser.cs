using AspNetCore.Identity.MongoDbCore.Models;
using MongoDbGenericRepository.Attributes;
using Swappa.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace Swappa.Entities.Models
{
    [CollectionName("Users")]
    public class AppUser : MongoIdentityUser<Guid>
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public Gender Gender { get; set; }
        public DateTime LastLogin { get; set; }
        public Status Status { get; set; } = Status.Inactive;
        public DateTime UpdatedOn { get; set; } = DateTime.Now;
        public DateTime DeactivatedOn { get; set; } = DateTime.MaxValue;
        public bool IsDeprecated { get; set; }
    }
}
