using MediatR;
using Swappa.Shared.DTOs;

namespace Swappa.Server.Commands.ContactMessage
{
    public class ToggleContactMessageIsReadCommand : IRequest<ResponseModel<bool>>
    {
        public Guid Id { get; set; }
    }
}