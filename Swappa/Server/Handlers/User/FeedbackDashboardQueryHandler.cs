using MediatR;
using Swappa.Data.Contracts;
using Swappa.Entities.Enums;
using Swappa.Entities.Responses;
using Swappa.Server.Queries.User;
using Swappa.Shared.DTOs;
using Swappa.Shared.Extensions;

namespace Swappa.Server.Handlers.User
{
    public class FeedbackDashboardQueryHandler : IRequestHandler<FeedbackDashboardQuery, ResponseModel<FeedbackDashboardDto>>
    {
        private readonly IRepositoryManager repository;
        private readonly ApiResponseDto response;

        public FeedbackDashboardQueryHandler(IRepositoryManager repository,
            ApiResponseDto response)
        {
            this.repository = repository;
            this.response = response;
        }

        public async Task<ResponseModel<FeedbackDashboardDto>> Handle(FeedbackDashboardQuery request, CancellationToken cancellationToken)
        {
            var feedbackCount = await repository.Feedback
                .Count(f => !f.IsDeprecated);

            var feedback = await Task.Run(() =>
                repository.Feedback
                    .FindAsQueryable(f => !f.IsDeprecated)
                    .Select(f => (int)f.Rating)
                    .ToList());

            var averageRating = feedback.IsNotNullOrEmpty() ?
                feedback.Average() : 0;
            
            return response
                .Process<FeedbackDashboardDto>(new ApiOkResponse<FeedbackDashboardDto>(new FeedbackDashboardDto
                {
                    TotalFeedbacks = feedbackCount,
                    AverageRating = ToFeedbackRating((int)Math.Round(averageRating, MidpointRounding.AwayFromZero))
                }));
        }

        private FeedbackRating ToFeedbackRating(int rating)
        {
            switch (rating)
            {
                case 0:
                case 1:
                    return FeedbackRating.VeryBad;
                case 2:
                    return FeedbackRating.Bad;
                case 3:
                    return FeedbackRating.Average;
                case 4:
                    return FeedbackRating.Good;
                default:
                    return FeedbackRating.Excellent;
            }
        }
    }
}
