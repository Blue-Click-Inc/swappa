using MediatR;
using Swappa.Shared.DTOs;

namespace Swappa.Server.Commands.Account
{
    public sealed record ForgotPasswordCommand : IRequest<ResponseModel<string>>
    {
        public ForgotPasswordDto Request { get; set; } = new ForgotPasswordDto();
    }
}
