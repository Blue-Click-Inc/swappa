using MediatR;
using Swappa.Shared.DTOs;

namespace Swappa.Server.Queries.Location
{
    public class GetEntityLocationQuery : IRequest<ResponseModel<BaseLocationDto>>
    {
        public Guid EntityId { get; set; }
    }
}
