using MediatR;
using Swappa.Shared.DTOs;
using Swappa.Shared.DTOs.Interfaces;

namespace Swappa.Server.Commands.Role
{
    public sealed record UpdateRoleCommand : RoleForUpdateDto, IBaseIdDto, IRequest<ResponseModel<string>>
    {
        public Guid Id { get; set; }
    }
}
