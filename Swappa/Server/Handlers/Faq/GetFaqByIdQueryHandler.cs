using MediatR;
using Swappa.Data.Contracts;
using Swappa.Entities.Responses;
using Swappa.Server.Queries.Faq;
using Swappa.Shared.DTOs;
using Swappa.Shared.Extensions;

namespace Swappa.Server.Handlers.Faq
{
    public class GetFaqByIdQueryHandler : IRequestHandler<GetFaqByIdQuery, ResponseModel<FaqToReturnDto>>
    {
        private readonly ApiResponseDto response;
        private readonly IRepositoryManager repository;

        public GetFaqByIdQueryHandler(ApiResponseDto response,
            IRepositoryManager repository)
        {
            this.response = response;
            this.repository = repository;
        }

        public async Task<ResponseModel<FaqToReturnDto>> Handle(GetFaqByIdQuery request, CancellationToken cancellationToken)
        {
            if (request.Id.IsEmpty())
            {
                return response.Process<FaqToReturnDto>(new BadRequestResponse("Invalid request"));
            }

            var faq = await repository.Faq.FindByIdAsync(request.Id);
            if (faq.IsNull())
            {
                return response.Process<FaqToReturnDto>(new NotFoundResponse("Could not find record with this Id"));
            }

            return response.Process<FaqToReturnDto>(new ApiOkResponse<FaqToReturnDto>(new FaqToReturnDto
            {
                Id = faq.Id,
                Title = faq.Title,
                Details = faq.Details,
                UpdatedAt = faq.UpdatedAt,
                CreatedAt = faq.CreatedAt,
                IsDeprecated = faq.IsDeprecated
            }));
        }
    }
}
