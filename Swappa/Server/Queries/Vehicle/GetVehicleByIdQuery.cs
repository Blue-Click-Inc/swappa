using MediatR;
using Swappa.Shared.DTOs;

namespace Swappa.Server.Queries.Vehicle
{
    public sealed class GetVehicleByIdQuery : IRequest<ResponseModel<VehicleToReturnDto>>
    {
        public Guid Id { get; set; }
    }
}
