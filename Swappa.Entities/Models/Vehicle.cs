using Mongo.Common;
using MongoDbGenericRepository.Attributes;
using Swappa.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace Swappa.Entities.Models
{
    [CollectionName("Vehicles")]
    public class Vehicle : IBaseEntity
    {
        public Guid Id { get; set; }
        [Required]
        public Guid UserId { get; set; }
        [Required]
        public string Make { get; set; }
        [Required]
        public string Model { get; set; }
        [Required]
        public int Year { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        public string Color { get; set; }
        public string Trim { get; set; }
        public Transmission Transmission { get; set; }
        public Engine Engine { get; set; }
        public DriveTrain DriveTrain { get; set; }
        public string VIN { get; set; }
        public string Interior { get; set; }
        public int Odometer { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsDeprecated { get; set; }
        public long Views { get; set; }
        public long Favorited { get; set; }
        public bool IsFavorite { get; set; }
        public bool IsSold { get; set; }
        public EntityLocation? Location { get; set; }
        public List<Image> Images { get; set; } = new();

        public Vehicle()
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
            Transmission = Transmission.None;
            Engine = Engine.None;
            DriveTrain = DriveTrain.None;
            VIN = string.Empty;
            Interior = string.Empty;
            Trim = string.Empty;
            Color = string.Empty;
            Model = string.Empty;
            Make = string.Empty;
        }
    }
}
