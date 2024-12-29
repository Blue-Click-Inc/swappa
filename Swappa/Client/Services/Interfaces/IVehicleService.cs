using Swappa.Shared.DTOs;

namespace Swappa.Client.Services.Interfaces
{
    public interface IVehicleService
    {
        Task<ResponseModel<string>?> AddAsync(MultipartFormDataContent request);
        Task<HttpResponseMessage?> ExportToExcel();
        Task<ResponseModel<VehicleToReturnDto>?> GetByIdAsync(Guid id);
        Task<ResponseModel<VehicleDashboardDto>?> GetDashboard();
        Task<ResponseModel<PaginatedListDto<VehicleToReturnDto>>?> GetDataAsync(VehicleQueryDto query);
        Task<ResponseModel<long>?> GetFavoriteCount(Guid userId);
        Task<ResponseModel<PaginatedListDto<VehicleToReturnDto>>?> GetFavoriteDataAsync(Guid userId, VehicleQueryDto query);
        Task<ResponseModel<bool>> IsFavorite(Guid id);
        Task<HttpResponseMessage?> PrintPDF();
        Task<ResponseModel<FavoriteVehicleResponseDto>?> ToggleFavorite(IdDto request);
        Task<ResponseModel<string>?> UpdateAsync(Guid id, VehicleForUpdateDto request);
    }
}
