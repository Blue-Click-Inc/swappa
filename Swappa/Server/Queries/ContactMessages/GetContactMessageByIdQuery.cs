using MediatR;
using Swappa.Shared.DTOs;

namespace Swappa.Server.Queries.ContactMessages
{
    public class GetContactMessageByIdQuery : IRequest<ResponseModel<ContactMessageToReturnDto>>
    {
        public Guid Id { get; set; }
    }
}
