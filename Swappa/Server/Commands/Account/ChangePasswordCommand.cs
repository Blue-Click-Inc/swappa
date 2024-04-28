using MediatR;
using Swappa.Shared.DTOs;
using Swappa.Shared.DTOs.Interfaces;

namespace Swappa.Server.Commands.Account
{
    public sealed record ChangePasswordCommand : IdDto, IRequest<ResponseModel<string>>
    {
        public ChangePasswordDto? Request { get; set; }
    }
}
