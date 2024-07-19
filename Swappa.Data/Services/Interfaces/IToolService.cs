using Hangfire.Server;
using Swappa.Entities.Models;

namespace Swappa.Data.Services.Interfaces
{
    public interface IToolService
    {
        Task VehicleBulkUpload(List<Vehicle> vehicles, List<Image> images, Guid userId, PerformContext context);
    }
}
