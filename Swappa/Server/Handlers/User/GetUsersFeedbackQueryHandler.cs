using AutoMapper;
using MediatR;
using Swappa.Data.Contracts;
using Swappa.Entities.Models;
using Swappa.Entities.Responses;
using Swappa.Server.Extensions;
using Swappa.Server.Queries.User;
using Swappa.Shared.DTOs;
using Swappa.Shared.Extensions;

namespace Swappa.Server.Handlers.User
{
    public class GetUsersFeedbackQueryHandler : IRequestHandler<GetUsersFeedbackQuery, ResponseModel<PaginatedListDto<UserFeedbacksDto>>>
    {
        private readonly IRepositoryManager repository;
        private readonly IMapper mapper;
        private readonly ApiResponseDto response;

        public GetUsersFeedbackQueryHandler(IRepositoryManager repository, 
            IMapper mapper, ApiResponseDto response)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.response = response;
        }

        public async Task<ResponseModel<PaginatedListDto<UserFeedbacksDto>>> Handle(GetUsersFeedbackQuery request, CancellationToken cancellationToken)
        {
            if (request.StartDate.IsLaterThan(request.EndDate))
            {
                return response
                    .Process<PaginatedListDto<UserFeedbacksDto>>(new BadRequestResponse("The End Date must be later than the Start Date"));
            }

            var feedbackQuery = await Task.Run(() => repository.Feedback
                .FindAsQueryable(f => !f.IsDeprecated)
                .FilterByDate(request.StartDate, request.EndDate));

            var feedbacks = PagedList<UserFeedback>.ToPagedList(feedbackQuery, request.PageNumber, request.PageSize);
            var feedbacksDictionary = mapper.Map<IEnumerable<UserFeedbackDto>>(feedbacks)
                .GroupBy(x => x.UserEmail)
                .ToDictionary(f => f.Key, f => f.ToList());

            var data = ToUserFeedbackDto(feedbacksDictionary) ?? new List<UserFeedbacksDto>();
            var ano = PagedList<UserFeedbacksDto>.ToPagedList(data, request.PageNumber, request.PageSize);
            var pagedDataFeedbacks = PaginatedListDto<UserFeedbacksDto>.Paginate(ano, ano.MetaData);
            return response.Process<PaginatedListDto<UserFeedbacksDto>>(new ApiOkResponse<PaginatedListDto<UserFeedbacksDto>>(pagedDataFeedbacks));
        }

        private List<UserFeedbacksDto>? ToUserFeedbackDto(Dictionary<string, List<UserFeedbackDto>>? feedbacks)
        {
            var result = new List<UserFeedbacksDto>();
            if(feedbacks != null)
            {
                foreach (var feedback in feedbacks)
                {
                    var feedbacksDto = new UserFeedbacksDto(feedback.Key, feedback.Value);
                    result.Add(feedbacksDto);
                }
            }
            return result?.OrderByDescending(f => f.Latest)
                .ToList();
        }
    }
}
