using MediatR;
using Swappa.Shared.DTOs;

namespace Swappa.Server.Queries.User
{
    public class GetFeedbacksByUserQuery : BasePageDto, IRequest<ResponseModel<PaginatedListDto<UserFeedbackDto>>>
    {
        public string UserEmail { get; set; } = string.Empty;
    }
}
