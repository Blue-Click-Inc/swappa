using MediatR;
using Swappa.Shared.DTOs;

namespace Swappa.Server.Queries.Faq
{
    public class GetAllFaqQuery : PageDto, IRequest<ResponseModel<PaginatedListDto<FaqToReturnDto>>>
    {
    }
}
