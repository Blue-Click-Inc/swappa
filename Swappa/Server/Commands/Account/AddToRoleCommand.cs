using MediatR;
using Swappa.Entities.Enums;
using Swappa.Shared.DTOs;

namespace Swappa.Server.Commands.Account
{
    public class AddToRoleCommand : IRequest<ResponseModel<string>>
    {
        public Guid UserId { get; set; }
        public SystemRole Role { get; set; }
    }
}
