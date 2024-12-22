using Swappa.Entities.Enums;

namespace Swappa.Shared.DTOs
{
    public class VehicleToReturnDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string? Make { get; set; }
        public string? Model { get; set; }
        public int Year { get; set; }
        public double Price { get; set; }
        public string? Color { get; set; }
        public string? Trim { get; set; }
        public Transmission Transmission { get; set; }
        public Engine Engine { get; set; }
        public DriveTrain DriveTrain { get; set; }
        public string? VIN { get; set; }
        public string? Interior { get; set; }
        public int Odometer { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsDeprecated { get; set; }
        public long Views { get; set; }
        public long Favorited { get; set; }
        public bool IsFavorite { get; set; }
        public LocationToReturnDto? Location { get; set; }
        public List<ImageToReturnDto> Images { get; set; } = new();
    }
}
