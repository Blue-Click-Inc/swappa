using MediatR;
using Swappa.Data.Contracts;
using Swappa.Entities.Responses;
using Swappa.Server.Commands.User;
using Swappa.Shared.DTOs;
using Swappa.Shared.Extensions;

namespace Swappa.Server.Handlers.User
{
    public class ToggleFeedbackDeprecationCommandHandler : IRequestHandler<ToggleFeedbackDeprecationCommand, ResponseModel<string>>
    {
        private readonly IRepositoryManager repository;
        private readonly ApiResponseDto response;

        public ToggleFeedbackDeprecationCommandHandler(IRepositoryManager repository,
            ApiResponseDto response)
        {
            this.repository = repository;
            this.response = response;
        }

        public async Task<ResponseModel<string>> Handle(ToggleFeedbackDeprecationCommand request, CancellationToken cancellationToken)
        {
            if (request == null || request.Id.IsEmpty())
            {
                return response.Process<string>(new BadRequestResponse($"Invalid request parameters."));
            }

            var feedback = await repository.Feedback
                .FindAsync(f => f.Id.Equals(request.Id));

            if (feedback == null)
            {
                return response.Process<string>(new BadRequestResponse($"No feedback record found with Id: {request.Id}."));
            }

            feedback.IsDeprecated = !feedback.IsDeprecated;
            await repository.Feedback
                .EditAsync(f => f.Id.Equals(request.Id), feedback);

            return response.Process<string>(new ApiOkResponse<string>("Feedback record successfully deleted."));
        }
    }
}
