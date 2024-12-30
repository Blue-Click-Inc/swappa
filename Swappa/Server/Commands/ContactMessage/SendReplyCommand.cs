using MediatR;
using Microsoft.Extensions.Primitives;
using Swappa.Shared.DTOs;

namespace Swappa.Server.Commands.ContactMessage
{
    public class SendReplyCommand : IRequest<ResponseModel<string>>
    {
        public ResponseMessageDto? Message { get; set; }
        public StringValues Origin { get; set; }
    }
}
