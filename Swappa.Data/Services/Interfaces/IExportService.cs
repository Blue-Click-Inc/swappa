using Swappa.Shared.DTOs;

namespace Swappa.Data.Services.Interfaces
{
    public interface IExportService
    {
        Task<byte[]> DownloadCarTemplate();
        Task<Stream> ExportVehicleDataToExcel();
        byte[] GeneratePDFSharp();
        byte[] TestPDF();
        Task<byte[]> VehiclesDetailsReport(DateRangeDto dateQuery);
        Task<Dictionary<string, double>> TestDict();
    }
}
