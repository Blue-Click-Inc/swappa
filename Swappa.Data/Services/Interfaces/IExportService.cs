namespace Swappa.Data.Services.Interfaces
{
    public interface IExportService
    {
        byte[] ExportToPdf();
        Task<Stream> ExportVehicleDataToExcel();
    }
}
