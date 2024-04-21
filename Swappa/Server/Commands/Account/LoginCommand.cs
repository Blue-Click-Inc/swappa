using MediatR;
using Swappa.Entities.Exceptions;
using Swappa.Shared.DTOs;

namespace Swappa.Server.Commands.Account
{
    public record LoginCommand : LoginDto, IRequest<ResponseModel<string>>
    {
    }
}