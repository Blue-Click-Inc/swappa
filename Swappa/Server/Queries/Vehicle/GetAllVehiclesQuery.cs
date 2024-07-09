using MediatR;
using Swappa.Shared.DTOs;

namespace Swappa.Server.Queries.Vehicle
{
    public class GetAllVehiclesQuery : VehicleQueryDto, IRequest<ResponseModel<PaginatedListDto<VehicleToReturnDto>>>
    {
    }
}
