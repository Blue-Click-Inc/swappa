using MediatR;
using Swappa.Shared.DTOs;

namespace Swappa.Server.Commands.Account
{
    public record RegisterCommand : RegisterDto, IRequest<ResponseModel<string>>
    {
    }
}
