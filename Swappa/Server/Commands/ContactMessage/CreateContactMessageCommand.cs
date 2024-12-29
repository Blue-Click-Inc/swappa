using MediatR;
using Swappa.Shared.DTOs;

namespace Swappa.Server.Commands.ContactMessage
{
    public class CreateContactMessageCommand : ContactMessageToAddDto, IRequest<ResponseModel<string>>
    {
    }
}