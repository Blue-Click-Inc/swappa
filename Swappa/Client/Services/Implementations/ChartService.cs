using Swappa.Client.Services.Interfaces;
using Swappa.Shared.DTOs;

namespace Swappa.Client.Services.Implementations
{
    public class ChartService : IChartService
    {
        private readonly HttpClient httpClient;
        private readonly HttpInterceptorService httpInterceptor;

        public ChartService(HttpClient httpClient,
            HttpInterceptorService httpInterceptor)
        {
            this.httpClient = httpClient;
            this.httpInterceptor = httpInterceptor;
        }
       
        public async Task<ResponseModel<TopFiveVehiclesChartsDto>?> GetTopFiveVehicleCharts()
        {
            var response = await httpClient.GetAsync($"charts/top-five");
            return await httpInterceptor.Process<ResponseModel<TopFiveVehiclesChartsDto>>(response);
        }

        public async Task<ResponseModel<PriceRangesAndEngineLineChartsDto>?> GetPriceRangeAndEngineCharts()
        {
            var response = await httpClient.GetAsync($"charts/price-range-and-engine");
            return await httpInterceptor.Process<ResponseModel<PriceRangesAndEngineLineChartsDto>>(response);
        }
    }
}
