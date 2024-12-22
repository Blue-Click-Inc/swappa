using Mongo.Common;

namespace Swappa.Entities.Models
{
    public class FavoriteVehicles : IBaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public bool IsDeprecated { get; set; }
        public Guid UserId { get; set; }
        public Guid VehicleId { get; set; }
    }
}
