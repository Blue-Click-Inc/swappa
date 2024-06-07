using MediatR;
using Swappa.Shared.DTOs;

namespace Swappa.Server.Queries.User
{
    public record GetUserByIdQuery : IdDto, IRequest<ResponseModel<UserDetailsDto>>
    {
    }
}
