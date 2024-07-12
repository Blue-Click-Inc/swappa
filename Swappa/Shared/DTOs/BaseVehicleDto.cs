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
        [Required,Range(1700, 9999, ErrorMessage = "Manufacturing year must be between 1700 and 9999.")]
        public int Year { get; set; }
        [Required, Range(1, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public double Price { get; set; }
        [Required]
        public string? Color { get; set; }
        public string? Trim { get; set; }
        [EnumDataType(typeof(Transmission))]
        public Transmission Transmission { get; set; }
        [EnumDataType(typeof(Engine))]
        public Engine Engine { get; set; }
        [EnumDataType(typeof(DriveTrain))]
        public DriveTrain DriveTrain { get; set; }
        public string? VIN { get; set; }
        public string? Interior { get; set; }
        public int Odometer { get; set; }
    }
}
