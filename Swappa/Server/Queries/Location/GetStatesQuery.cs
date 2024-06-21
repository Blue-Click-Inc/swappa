using MediatR;
using Swappa.Shared.DTOs;

namespace Swappa.Server.Queries.Location
{
    public class GetStatesQuery : IRequest<ResponseModel<List<StateDataToReturnDto>>>
    {
        public string? CountryId { get; set; }
    }
}
