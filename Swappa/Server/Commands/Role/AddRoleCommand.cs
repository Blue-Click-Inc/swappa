using MediatR;
using Swappa.Shared.DTOs;

namespace Swappa.Server.Commands.Role
{
    public record AddRoleCommand : RoleForCreateDto, IRequest<ResponseModel<string>>
    {
    }
}
