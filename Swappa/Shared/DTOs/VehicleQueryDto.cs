using Swappa.Entities.Enums;

namespace Swappa.Shared.DTOs
{
    public class VehicleQueryDto : PageDto
    {
        private readonly int minYear = 1700;
        private readonly int maxYear = 9999;
        private readonly double minPrice = default;
        private readonly double maxPrice = double.MaxValue;

        private int _year = default;
        private double _price = default;

        private int _maxYear = default;
        private double _maxPrice = default;

        public string SearchTerm { get; set; } = string.Empty;
        public int MinYear 
        { 
            get { return _year;  }
            set { _year = minYear > value || 
                    maxYear < value ? _year : value; 
                }
        }
        public int MaxYear
        {
            get { return _maxYear; }
            set { _maxYear = minYear > value || 
                    maxYear < value ? _maxYear : value; 
                }
        }
        public double MinPrice 
        { 
            get { return _price; }
            set { _price = minPrice > value || 
                    maxPrice < value ? _price : value; 
                } 
        }
        public double MaxPrice
        {
            get { return _maxPrice; }
            set { _maxPrice = minPrice > value || 
                    maxPrice < value ? _maxPrice : value; 
                }
        }
        public bool IncludeSold { get; set; }
        public Transmission Transmission { get; set; } = Transmission.None;
        public Engine Engine { get; set; } = Engine.None;
        public DriveTrain DriveTrain { get; set; } = DriveTrain.None;
    }
}
