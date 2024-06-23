using MediatR;
using Swappa.Shared.DTOs;

namespace Swappa.Server.Commands.Location
{
    public class UpdateLocationCommand : BaseLocationDto, IRequest<ResponseModel<string>>
    {
    }
}
