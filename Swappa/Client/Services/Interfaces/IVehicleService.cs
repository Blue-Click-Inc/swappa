using Swappa.Shared.DTOs;

namespace Swappa.Client.Services.Interfaces
{
    public interface IVehicleService
    {
        Task<ResponseModel<string>?> AddAsync(VehicleToCreateDto request);
        Task<HttpResponseMessage?> ExportToExcel();
        Task<ResponseModel<VehicleToReturnDto>?> GetByIdAsync(Guid id);
        Task<ResponseModel<VehicleDashboardDto>?> GetDashboard();
        Task<ResponseModel<PaginatedListDto<VehicleToReturnDto>>?> GetDataAsync(VehicleQueryDto query);
        Task<HttpResponseMessage?> PrintPDF();
        Task<ResponseModel<string>?> UpdateAsync(Guid id, VehicleForUpdateDto request);
    }
}
