using Swappa.Shared.DTOs;

namespace Swappa.Data.Services.Interfaces
{
    public interface IExportService
    {
        Task<Stream> ExportVehicleDataToExcel();
        Task<byte[]> VehiclesDetailsReport(DateRangeDto dateQuery);
    }
}
