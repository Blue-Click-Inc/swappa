using MediatR;
using Swappa.Data.Contracts;
using Swappa.Entities.Responses;
using Swappa.Server.Queries.Charts;
using Swappa.Shared.DTOs;
using Swappa.Shared.Extensions;

namespace Swappa.Server.Handlers.Charts
{
    public class FeedbackPiechartQueryHandler : IRequestHandler<FeedbackPiechartQuery, ResponseModel<Dictionary<string, double?>>>
    {
        private readonly ApiResponseDto response;
        private readonly IRepositoryManager repository;

        public FeedbackPiechartQueryHandler(ApiResponseDto response,
            IRepositoryManager repository)
        {
            this.response = response;
            this.repository = repository;
        }

        public async Task<ResponseModel<Dictionary<string, double?>>> Handle(FeedbackPiechartQuery request, CancellationToken cancellationToken)
        {
            var data = await Task.Run(() => 
                repository.Feedback.FindAsQueryable(f => !f.IsDeprecated)
                    .GroupBy(f => f.Rating)
                    .ToDictionary(k => k.Key.GetDescription(), v => (double?)v.LongCount()));

            return response.Process<Dictionary<string, double?>>(
                new ApiOkResponse<Dictionary<string, double?>>(data));
        }
    }
}
