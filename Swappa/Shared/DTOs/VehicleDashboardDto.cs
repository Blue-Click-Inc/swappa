using Swappa.Entities.Enums;

namespace Swappa.Shared.DTOs
{
    public class VehicleDashboardDto
    {
        public long TotalCount { get; set; }
        public Dictionary<Engine, int> Engine { get; set; }
        public Dictionary<Transmission, int> Transmission { get; set; }
        public Dictionary<DriveTrain, int> DriveTrain { get; set; }

        public VehicleDashboardDto()
        {
            Transmission = new Dictionary<Transmission, int>();
            Engine = new Dictionary<Engine, int>();
            DriveTrain = new Dictionary<DriveTrain, int>();
        }
    }
}
