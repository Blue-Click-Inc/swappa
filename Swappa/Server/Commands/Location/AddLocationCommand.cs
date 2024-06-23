using MediatR;
using Swappa.Shared.DTOs;

namespace Swappa.Server.Commands.Location
{
    public class AddLocationCommand : BaseLocationDto, IRequest<ResponseModel<string>>
    {   
    }
}
