using Swappa.Entities.Enums;

namespace Swappa.Shared.DTOs
{
    public class VehiclesReportDto
    {
        public string? MerchantName { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public double TotalPrice { get; set; }
        public int NumOfVehicles { get; set; }
        public double LowestPriced { get; set; }
        public double HighestPrice { get; set; }
        public double AveragePrice { get; set; }
        public Dictionary<Engine, int> Engine { get; set; }
        public Dictionary<Transmission, int> Transmission { get; set; }
        public Dictionary<DriveTrain, int> DriveTrain { get; set; }

        public VehiclesReportDto()
        {
            Transmission = new Dictionary<Transmission, int>();
            DriveTrain = new Dictionary<DriveTrain, int>();
            Engine = new Dictionary<Engine, int>();
        }
    }
}
