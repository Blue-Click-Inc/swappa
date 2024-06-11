using MediatR;
using Swappa.Shared.DTOs;

namespace Swappa.Server.Queries.User
{
    public class GetUsersFeedbackQuery : PageAndDateDto, IRequest<ResponseModel<PaginatedListDto<UserFeedbackCountDto>>>
    {
    }
}