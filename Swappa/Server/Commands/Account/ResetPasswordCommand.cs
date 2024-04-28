using MediatR;
using Swappa.Shared.DTOs;

namespace Swappa.Server.Commands.Account
{
    public sealed record ResetPasswordCommand : IRequest<ResponseModel<string>>
    {
        public EmailDto? Request { get; set; }
    }
}
