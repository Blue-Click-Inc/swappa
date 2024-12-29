using MediatR;
using Swappa.Shared.DTOs;

namespace Swappa.Server.Queries.Charts
{
    public class GetTopFiveVehiclesChartQuery : IRequest<ResponseModel<TopFiveVehiclesChartsDto>>
    {
    }
}
