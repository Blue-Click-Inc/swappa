using Mongo.Common;
using System.ComponentModel.DataAnnotations;

namespace Swappa.Entities.Models
{
    public class Image : IBaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required]
        public Guid VehicleId { get; set; }
        [Required, Url]
        public string Url { get; set; } = string.Empty;
        public string PublicId { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public bool IsDeprecated { get; set; }
        public bool IsMain { get; set; }
    }
}
