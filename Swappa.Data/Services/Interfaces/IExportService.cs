using Swappa.Shared.DTOs;

namespace Swappa.Data.Services.Interfaces
{
    public interface IExportService
    {
        byte[] ExportToPdf();
        Task<Stream> ExportVehicleDataToExcel();
        Task<byte[]> VehiclesDetailsReport(DateRangeDto dateQuery);
    }
}
