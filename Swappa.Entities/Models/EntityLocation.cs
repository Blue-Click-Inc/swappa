using Mongo.Common;
using Swappa.Entities.Enums;

namespace Swappa.Entities.Models
{
    public class EntityLocation : IBaseEntity
    {
        public Guid Id { get; set; }
        public Guid EntityId { get; set; }
        public string CountryId { get; set; } = string.Empty;
        public string StateId { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public Coordinate Coordinate { get; set; }
        public EntityType EntityType { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsDeprecated { get; set; }

        public EntityLocation()
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
            Coordinate = new Coordinate();
        }
    }

    public class Coordinate
    {
        public string Longitude { get; set; } = string.Empty;
        public string Latitude { get; set; } = string.Empty;
    }
}
