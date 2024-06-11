using AutoMapper;
using MediatR;
using Swappa.Data.Contracts;
using Swappa.Entities.Models;
using Swappa.Entities.Responses;
using Swappa.Server.Queries.User;
using Swappa.Server.Validations.User;
using Swappa.Shared.DTOs;

namespace Swappa.Server.Handlers.User
{
    public class GetFeedbacksByUserQueryHandler : IRequestHandler<GetFeedbacksByUserQuery, ResponseModel<PaginatedListDto<UserFeedbackDto>>>
    {
        private readonly IRepositoryManager repository;
        private readonly IMapper mapper;
        private readonly ApiResponseDto response;

        public GetFeedbacksByUserQueryHandler(IRepositoryManager repository, 
            IMapper mapper, ApiResponseDto response)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.response = response;
        }

        public async Task<ResponseModel<PaginatedListDto<UserFeedbackDto>>> Handle(GetFeedbacksByUserQuery request, CancellationToken cancellationToken)
        {
            var validator = new EmailAddressValidator();
            var validationResult = validator.Validate(request.UserEmail);
            if (!validationResult.IsValid)
            {
                return response
                    .Process<PaginatedListDto<UserFeedbackDto>>(new BadRequestResponse(validationResult.Errors?.FirstOrDefault()?.ErrorMessage ?? string.Empty));
            }

            var feedbackQuery = repository.Feedback
                .FindAsQueryable(f => !f.IsDeprecated && f.UserEmail.Equals(request.UserEmail))
                .OrderByDescending(f => f.CreatedAt);

            var pagedList = await Task.Run(() => 
                PagedList<UserFeedback>.ToPagedList(feedbackQuery, request.PageNumber, request.PageSize));

            var feedbacks = mapper.Map<IEnumerable<UserFeedbackDto>>(pagedList);

            var pagedDataFeedbacks = PaginatedListDto<UserFeedbackDto>.Paginate(feedbacks, pagedList.MetaData);
            return response.Process<PaginatedListDto<UserFeedbackDto>>(new ApiOkResponse<PaginatedListDto<UserFeedbackDto>>(pagedDataFeedbacks));
        }
    }
}
