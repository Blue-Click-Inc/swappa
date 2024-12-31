using MediatR;
using Swappa.Shared.DTOs;

namespace Swappa.Server.Commands.ContactMessage
{
    public class DeprecateMessageCommand : IRequest<ResponseModel<string>>
    {
        public Guid Id { get; set; }
    }
}
