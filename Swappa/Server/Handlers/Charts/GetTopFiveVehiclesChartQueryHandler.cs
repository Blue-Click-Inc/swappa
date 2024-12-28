using MediatR;
using Swappa.Data.Contracts;
using Swappa.Entities.Responses;
using Swappa.Server.Queries.Charts;
using Swappa.Shared.DTOs;

namespace Swappa.Server.Handlers.Charts
{
    public class GetTopFiveVehiclesChartQueryHandler : IRequestHandler<GetTopFiveVehiclesChartQuery, ResponseModel<TopFiveVehiclesChartsDto>>
    {
        private readonly ApiResponseDto response;
        private readonly IRepositoryManager repository;

        public GetTopFiveVehiclesChartQueryHandler(ApiResponseDto response,
            IRepositoryManager repository)
        {
            this.response = response;
            this.repository = repository;
        }

        public async Task<ResponseModel<TopFiveVehiclesChartsDto>> Handle(GetTopFiveVehiclesChartQuery request, CancellationToken cancellationToken)
        {
            var currentYear = DateTime.UtcNow.Year;
            var chartsData = await Task.Run(() => repository.Vehicle.FindAsQueryable(v => !v.IsDeprecated &&
                v.CreatedAt.Year.Equals(currentYear))
                .ToList()
                .GroupBy(v => v.Make.ToUpper())
                .ToDictionary(group => group.Key, v => (double?)v.LongCount()));

            var chartsDatas = chartsData.OrderByDescending(c => c.Value).Take(5).ToDictionary(k => k.Key, v => v.Value);
            return response.Process<TopFiveVehiclesChartsDto>(new ApiOkResponse<TopFiveVehiclesChartsDto>(new TopFiveVehiclesChartsDto
            {
                TopFiveVehicles = chartsDatas
            }));
        }
    }
}