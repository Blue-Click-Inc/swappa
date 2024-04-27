using MediatR;
using Swappa.Shared.DTOs;

namespace Swappa.Server.Commands.Role
{
    public record DeleteRoleCommand : IdDto, IRequest<ResponseModel<string>>
    {
    }
}
