using Swappa.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace Swappa.Shared.DTOs
{
    public abstract class BaseVehicleDto
    {
        [Required]
        public string? Make { get; set; }
        [Required]
        public string? Model { get; set; }
        [Required]
        public int Year { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        public string? Color { get; set; }
        public string? Trim { get; set; }
        public Transmission Transmission { get; set; }
        public Engine Engine { get; set; }
        public DriveTrain DriveTrain { get; set; }
        public string? VIN { get; set; }
        public string? Interior { get; set; }
        public int Odometer { get; set; }
    }
}
