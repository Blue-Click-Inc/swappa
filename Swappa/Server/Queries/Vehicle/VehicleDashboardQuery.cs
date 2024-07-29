using MediatR;
using Swappa.Shared.DTOs;

namespace Swappa.Server.Queries.Vehicle
{
    public class VehicleDashboardQuery : IRequest<ResponseModel<VehicleDashboardDto>>
    {
    }
}
