﻿using Swappa.Shared.DTOs;

namespace Swappa.Client.Services.Interfaces
{
    public interface IVehicleService
    {
        Task<HttpResponseMessage?> ExportToExcel();
        Task<ResponseModel<VehicleDashboardDto>?> GetDashboard();
        Task<ResponseModel<PaginatedListDto<VehicleToReturnDto>>?> GetDataAsync(VehicleQueryDto query);
        Task<HttpResponseMessage?> PrintPDF();
    }
}
