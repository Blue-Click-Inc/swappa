namespace Swappa.Data.Services.Interfaces
{
    public interface IExportService
    {
        Task<Stream> ExportVehicleDataToExcel();
    }
}
