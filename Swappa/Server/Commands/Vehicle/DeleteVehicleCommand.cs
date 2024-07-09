using MediatR;
using Swappa.Shared.DTOs;

namespace Swappa.Server.Commands.Vehicle
{
    public class DeleteVehicleCommand : IRequest<ResponseModel<string>>
    {
        public Guid Id { get; set; }
    }
}
