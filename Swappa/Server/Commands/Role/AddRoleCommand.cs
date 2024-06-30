using MediatR;
using Swappa.Shared.DTOs;

namespace Swappa.Server.Commands.Role
{
    public class AddRoleCommand : RoleForCreateDto, IRequest<ResponseModel<string>>
    {
    }
}
