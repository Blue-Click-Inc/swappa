using MediatR;
using Swappa.Shared.DTOs;

namespace Swappa.Server.Commands.Account
{
    public record LoginCommand : LoginDto, IRequest<ResponseModel<string>>
    {
    }
}