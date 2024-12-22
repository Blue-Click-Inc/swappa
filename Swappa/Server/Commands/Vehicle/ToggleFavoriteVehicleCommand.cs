using MediatR;
using Swappa.Shared.DTOs;

namespace Swappa.Server.Commands.Vehicle
{
    public class ToggleFavoriteVehicleCommand : IRequest<ResponseModel<FavoriteVehicleResponseDto>>
    {
        public Guid VehicleId { get; set; }
    }
}
