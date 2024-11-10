using MediatR;
using Swappa.Shared.DTOs;

namespace Swappa.Server.Commands.Vehicle
{
    public sealed class AddVehicleCommand : IRequest<ResponseModel<string>>
    {
        public VehicleToCreateDto Request { get; set; } = new();
    }
}
