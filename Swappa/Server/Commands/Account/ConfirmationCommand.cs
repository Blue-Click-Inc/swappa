using MediatR;
using Swappa.Shared.DTOs;

namespace Swappa.Server.Commands.Account
{
    public record ConfirmationCommand : AccountConfirmationDto, IRequest<ResponseModel<string>>
    {
    }
}
