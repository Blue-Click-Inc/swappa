using MediatR;
using Swappa.Shared.DTOs;
using Swappa.Shared.DTOs.Interfaces;

namespace Swappa.Server.Commands.Account
{
    public class ToggleStatusCommand : IBaseIdDto, IRequest<ResponseModel<string>>
    {
        public Guid Id { get; set; }
    }
}
