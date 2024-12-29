using MediatR;
using Swappa.Shared.DTOs;

namespace Swappa.Server.Queries.Charts
{
    public class FeedbackPiechartQuery : IRequest<ResponseModel<Dictionary<string, double?>>>
    {
    }
}
