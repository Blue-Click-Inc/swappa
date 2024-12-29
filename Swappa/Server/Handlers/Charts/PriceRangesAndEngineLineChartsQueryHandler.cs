using MediatR;
using Swappa.Data.Contracts;
using Swappa.Entities.Enums;
using Swappa.Entities.Responses;
using Swappa.Server.Queries.Charts;
using Swappa.Shared.DTOs;
using Swappa.Shared.Extensions;

namespace Swappa.Server.Handlers.Charts
{
    public class PriceRangesAndEngineLineChartsQueryHandler : IRequestHandler<PriceRangesAndEngineLineChartsQuery, ResponseModel<PriceRangesAndEngineLineChartsDto>>
    {
        private readonly ApiResponseDto response;
        private readonly IRepositoryManager repository;

        private const string LESSTHAN1M = "< 1M";
        private const string BETWEEN1AND5M = "1M - 5M";
        private const string BETWEEN5AND10M = "5M - 10M";
        private const string BETWEEN10AND20M = "10M - 20M";
        private const string BETWEEN20AND50M = "20M - 50M";
        private const string GREATERTHAN50M = "> 50M";
        private const double ONEM = 1000000;
        private const double FIVEM = 5000000;
        private const double TENM = 10000000;
        private const double TWENTYM = 20000000;
        private const double FIFTYM = 50000000;

        public PriceRangesAndEngineLineChartsQueryHandler(ApiResponseDto response,
            IRepositoryManager repository)
        {
            this.response = response;
            this.repository = repository;
        }

        public async Task<ResponseModel<PriceRangesAndEngineLineChartsDto>> Handle(PriceRangesAndEngineLineChartsQuery request, CancellationToken cancellationToken)
        {
            var groupings = await Task.Run(() => 
                repository.Vehicle.FindAsQueryable(v => !v.IsDeprecated &&
                            v.CreatedAt.Year.Equals(DateTime.UtcNow.Year))
                .GroupBy(v => v.Engine)
                .ToDictionary(k => k.Key, v => v.ToList()));

            var result = new PriceRangesAndEngineLineChartsDto();
            result.Labels = InitializeLabels();

            foreach (var kvp in groupings)
            {
                var lessThan1m = kvp.Value.Where(v => v.Price < ONEM)
                    .Count();

                var lessThan5m = kvp.Value.Where(v => v.Price >= ONEM && v.Price < FIVEM)
                    .Count();

                var lessThan10m = kvp.Value.Where(v => v.Price >= FIVEM && v.Price < TENM)
                    .Count();

                var lessThan20m = kvp.Value.Where(v => v.Price >= TENM && v.Price < TWENTYM)
                    .Count();

                var lessThan50m = kvp.Value.Where(v => v.Price >= TWENTYM && v.Price < FIFTYM)
                    .Count();

                var greaterThan50m = kvp.Value.Where(v => v.Price >= FIFTYM)
                    .Count();

                result.LineCharts.Add(new LineChartsDto
                {
                    Label = kvp.Key.GetDescription(),
                    Data = new List<double?>
                    {
                        lessThan1m, lessThan5m, lessThan10m, 
                        lessThan20m, lessThan50m, greaterThan50m
                    }
                });
            }

            return response.Process<PriceRangesAndEngineLineChartsDto>(
                new ApiOkResponse<PriceRangesAndEngineLineChartsDto>(result));
        }

        private static List<string> InitializeLabels()
        {
            return new List<string>
            {
                LESSTHAN1M,
                BETWEEN1AND5M,
                BETWEEN5AND10M,
                BETWEEN10AND20M,
                BETWEEN20AND50M,
                GREATERTHAN50M
            };
        }

        private static List<LineChartsDto> InitializeLineChartData(List<Engine> keys)
        {
            var result = new List<LineChartsDto>();
            foreach (var item in keys)
            {
                result.Add(new LineChartsDto
                {
                    Label = item.GetDescription(),
                    Data = new List<double?>()
                });
            }

            return result;
        }
    }
}
