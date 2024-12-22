using MediatR;
using Swappa.Shared.DTOs;

namespace Swappa.Server.Queries.Vehicle
{
    public class GetFavoriteVehicleCountQuery : IRequest<ResponseModel<long>>
    {
        public Guid UserId { get; set; }
    }
}
