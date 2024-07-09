using Microsoft.AspNetCore.Http;

namespace Swappa.Shared.DTOs
{
    public class VehicleToCreateDto : BaseVehicleDto
    {
        public VehicleToCreateDto()
        {
            Images = new List<IFormFile>();
        }

        public List<IFormFile> Images { get; set; }
    }
}
