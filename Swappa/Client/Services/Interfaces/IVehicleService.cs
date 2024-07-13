using Swappa.Shared.DTOs;

namespace Swappa.Client.Services.Interfaces
{
    public interface IVehicleService
    {
        Task<ResponseModel<PaginatedListDto<VehicleToReturnDto>>?> GetDataAsync(VehicleQueryDto query);
    }
}
