using MediatR;
using Swappa.Shared.DTOs;

namespace Swappa.Server.Commands.Vehicle
{
    public sealed class AddVehicleCommand : VehicleToCreateDto, IRequest<ResponseModel<string>>
    {
    }
}
