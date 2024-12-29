using MediatR;
using Swappa.Shared.DTOs;

namespace Swappa.Server.Queries.Vehicle
{
    public class GetFavoriteVehiclesQuery : IRequest<ResponseModel<PaginatedListDto<VehicleToReturnDto>>>
    {
        public Guid UserId { get; set; }
        public PageDto Request { get; set; } = new();
    }
}
