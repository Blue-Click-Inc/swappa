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
    public class GetUsersFeedbackQueryHandler : IRequestHandler<GetUsersFeedbackQuery, ResponseModel<PaginatedListDto<UserFeedbackCountDto>>>
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

        public async Task<ResponseModel<PaginatedListDto<UserFeedbackCountDto>>> Handle(GetUsersFeedbackQuery request, CancellationToken cancellationToken)
        {
            if (request.StartDate.IsLaterThan(request.EndDate))
            {
                return response
                    .Process<PaginatedListDto<UserFeedbackCountDto>>(new BadRequestResponse("The End Date must be later than the Start Date"));
            }

            var userFeedbackDistinctEmailQuery = await Task.Run(() =>
                repository.Feedback
                    .FindAsQueryable(f => !f.IsDeprecated)
                    .FilterByDate(request.StartDate, request.EndDate)
                    .OrderByDescending(f => f.CreatedAt)
                    .GroupBy(f => f.UserEmail)
                    .Skip((request.PageNumber - 1) * request.PageSize)
                    .Take(request.PageSize));

            var userFeedbackDistinctEmail = userFeedbackDistinctEmailQuery
                .Select(x => x.Key).ToList();

            var feedbacks = repository.Feedback
                    .FindAsQueryable(f => !f.IsDeprecated && userFeedbackDistinctEmail.Contains(f.UserEmail))
                    .FilterByDate(request.StartDate, request.EndDate);

            var feedbacksDictionary = await Task.Run(() =>
                mapper.Map<IEnumerable<UserFeedbackDto>>(feedbacks)
                    .GroupBy(x => x.UserEmail)
                    .ToDictionary(f => f.Key, f => f.ToList()));

            var data = ToUserFeedbackDto(feedbacksDictionary) ?? new List<UserFeedbackCountDto>();
            var pagedDataFeedbacks = PaginatedListDto<UserFeedbackCountDto>.Paginate(data, new MetaData(userFeedbackDistinctEmail.Count, request.PageSize, request.PageNumber));
            return response.Process<PaginatedListDto<UserFeedbackCountDto>>(new ApiOkResponse<PaginatedListDto<UserFeedbackCountDto>>(pagedDataFeedbacks));
        }

        private List<UserFeedbackCountDto>? ToUserFeedbackDto(Dictionary<string, List<UserFeedbackDto>>? feedbacks)
        {
            var result = new List<UserFeedbackCountDto>();
            if(feedbacks != null)
            {
                foreach (var feedback in feedbacks)
                {
                    var feedbacksDto = new UserFeedbackCountDto(feedback.Key, feedback.Value);
                    result.Add(feedbacksDto);
                }
            }
            return result?.OrderByDescending(f => f.Latest)
                .ToList();
        }
    }
}
