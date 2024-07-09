using MediatR;
using Swappa.Shared.DTOs;

namespace Swappa.Server.Commands.Vehicle
{
    public sealed class UpdateVehicleCommand : IRequest<ResponseModel<string>>
    {
        public Guid Id { get; set; }
        public VehicleForUpdateDto Request { get; set; } = new();
    }
}
