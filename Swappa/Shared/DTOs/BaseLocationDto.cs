using Swappa.Entities.Enums;

namespace Swappa.Shared.DTOs
{
    public class BaseLocationDto
    {
        public Guid EntityId { get; set; }
        public EntityType EntityType { get; set; }
        public string CountryId { get; set; } = string.Empty;
        public string StateId { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
    }
}
