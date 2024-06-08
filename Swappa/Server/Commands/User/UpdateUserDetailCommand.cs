using MediatR;
using Swappa.Shared.DTOs;

namespace Swappa.Server.Commands.User
{
    public record UpdateUserDetailCommand : IRequest<ResponseModel<string>>
    {
        public Guid UserId { get; set; }
        public UserDetailsForUpdateDto? Command { get; set; }
    }
}
