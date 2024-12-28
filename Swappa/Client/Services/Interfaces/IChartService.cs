using Swappa.Shared.DTOs;

namespace Swappa.Client.Services.Interfaces
{
    public interface IChartService
    {
        Task<ResponseModel<PriceRangesAndEngineLineChartsDto>?> GetPriceRangeAndEngineCharts();
        Task<ResponseModel<TopFiveVehiclesChartsDto>?> GetTopFiveVehicleCharts();
    }
}
