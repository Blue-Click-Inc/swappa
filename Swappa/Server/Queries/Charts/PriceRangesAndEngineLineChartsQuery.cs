using MediatR;
using Swappa.Shared.DTOs;

namespace Swappa.Server.Queries.Charts
{
    public class PriceRangesAndEngineLineChartsQuery : IRequest<ResponseModel<PriceRangesAndEngineLineChartsDto>>
    {
    }
}
