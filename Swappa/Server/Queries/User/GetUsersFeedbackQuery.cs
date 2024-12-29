using MediatR;
using Swappa.Shared.DTOs;

namespace Swappa.Server.Queries.User
{
    public class GetUsersFeedbackQuery : IRequest<ResponseModel<PaginatedListDto<UserFeedbackDto>>>
    {
        public PageDto Dto { get; set; } = new();
    }
}