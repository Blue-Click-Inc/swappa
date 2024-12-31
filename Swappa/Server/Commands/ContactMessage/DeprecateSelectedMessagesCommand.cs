using MediatR;
using Swappa.Shared.DTOs;

namespace Swappa.Server.Commands.ContactMessage
{
    public class DeprecateSelectedMessagesCommand : IRequest<ResponseModel<string>>
    {
        public List<Guid> SelectedIds { get; set; } = new();
    }
}
