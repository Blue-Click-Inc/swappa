using MediatR;
using Swappa.Shared.DTOs;

namespace Swappa.Server.Queries.Role
{
    public record GetRolebyIdQuery : IdDto, IRequest<ResponseModel<RoleDto>>
    {
    }
}
