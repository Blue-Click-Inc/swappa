using MediatR;
using Swappa.Data.Contracts;
using Swappa.Entities.Models;
using Swappa.Entities.Responses;
using Swappa.Server.Extensions;
using Swappa.Server.Queries.Faq;
using Swappa.Shared.DTOs;

namespace Swappa.Server.Handlers.Faq
{
    public class GetAllFaqQueryHandler : IRequestHandler<GetAllFaqQuery, ResponseModel<PaginatedListDto<FaqToReturnDto>>>
    {
        private readonly ApiResponseDto response;
        private readonly IRepositoryManager repository;

        public GetAllFaqQueryHandler(ApiResponseDto response,
            IRepositoryManager repository)
        {
            this.response = response;
            this.repository = repository;
        }

        public async Task<ResponseModel<PaginatedListDto<FaqToReturnDto>>> Handle(GetAllFaqQuery request, CancellationToken cancellationToken)
        {
            var query = repository.Faq.FindAsQueryable(v => !v.IsDeprecated)
                .OrderByDescending(v => v.CreatedAt)
                .Search(request.SearchTerm);

            var pagedList = await Task.Run(() => 
                PagedList<Entities.Models.Faq>.ToPagedList(query, request.PageNumber, request.PageSize));

            var pagedData = PaginatedListDto<FaqToReturnDto>.Paginate(ToFaqReturnDto(pagedList), pagedList.MetaData);
            return response.Process<PaginatedListDto<FaqToReturnDto>>(new ApiOkResponse<PaginatedListDto<FaqToReturnDto>>(pagedData));
        }

        private List<FaqToReturnDto> ToFaqReturnDto(PagedList<Entities.Models.Faq> pagedList)
        {
            var result = new List<FaqToReturnDto>();
            pagedList.ForEach(item =>
            {
                result.Add(new FaqToReturnDto
                {
                    Id = item.Id,
                    Title = item.Title,
                    Details = item.Details,
                    CreatedAt = item.CreatedAt,
                    UpdatedAt = item.UpdatedAt,
                    IsDeprecated = item.IsDeprecated
                });
            });

            return result;
        }
    }
}
