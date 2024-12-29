using MediatR;
using Swappa.Shared.DTOs;

namespace Swappa.Server.Queries.User
{
    public class GetPagedUserListQuery : IRequest<ResponseModel<PaginatedListDto<LeanUserDetailsDto>>>
    {
        public PageDto Query { get; set; } = new();
    }
}
