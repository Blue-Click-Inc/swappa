using Swappa.Entities.Enums;

namespace Swappa.Shared.DTOs
{
    public class VehicleQueryDto : PageDto
    {
        private readonly int minYear = 1700;
        private readonly int maxYear = 9999;
        private readonly double minPrice = 0;
        private readonly double maxPrice = double.MaxValue;

        private int _year = 0;
        private double _price = 0;

        public string SearchTerm { get; set; } = string.Empty;
        public int Year 
        { 
            get { return _year;  }
            set { _year = minYear > value || maxYear < value ? _year : value; }
        }
        public double Price 
        { 
            get { return _price; }
            set { _price = minPrice > value || maxPrice < value ? maxPrice : value; } 
        }
        public Transmission Transmission { get; set; } = Transmission.None;
        public Engine Engine { get; set; } = Engine.None;
        public DriveTrain DriveTrain { get; set; } = DriveTrain.None;
    }
}
