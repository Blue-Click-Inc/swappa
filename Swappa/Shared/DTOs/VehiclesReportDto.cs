using Swappa.Entities.Enums;
using Swappa.Entities.Models;

namespace Swappa.Shared.DTOs
{
    public class VehiclesReportDto
    {
        public string? MerchantName { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public double TotalPrice { get; set; }
        public int NumOfVehicles { get; set; }
        public VehicleReportContactDetails Contact { get; set; }
        public Dictionary<Engine, List<Vehicle>> Engine { get; set; }
        public Dictionary<Transmission, List<Vehicle>> Transmission { get; set; }
        public Dictionary<DriveTrain, List<Vehicle>> DriveTrain { get; set; }

        public VehiclesReportDto()
        {
            Transmission = new Dictionary<Transmission, List<Vehicle>>();
            DriveTrain = new Dictionary<DriveTrain, List<Vehicle>>();
            Engine = new Dictionary<Engine, List<Vehicle>>();
            Contact = new VehicleReportContactDetails();
        }
    }

    public class VehicleReportContactDetails
    {
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }
        public string? PostalCode { get; set; }
    }
}
