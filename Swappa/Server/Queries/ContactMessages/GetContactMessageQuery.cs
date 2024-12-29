using MediatR;
using Swappa.Shared.DTOs;

namespace Swappa.Server.Queries.ContactMessages
{
    public class GetContactMessageQuery : IRequest<ResponseModel<PaginatedListDto<ContactMessageToReturnDto>>>
    {
        public PageDto Query { get; set; } = new();
    }
}
