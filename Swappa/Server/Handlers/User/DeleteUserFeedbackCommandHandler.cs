using MediatR;
using Swappa.Data.Contracts;
using Swappa.Entities.Responses;
using Swappa.Server.Commands.User;
using Swappa.Shared.DTOs;
using Swappa.Shared.Extensions;

namespace Swappa.Server.Handlers.User
{
    public class DeleteUserFeedbackCommandHandler : IRequestHandler<DeleteUserFeedbackCommand, ResponseModel<string>>
    {
        private readonly IRepositoryManager repository;
        private readonly ApiResponseDto response;

        public DeleteUserFeedbackCommandHandler(IRepositoryManager repository,
            ApiResponseDto response)
        {
            this.repository = repository;
            this.response = response;
        }

        public async Task<ResponseModel<string>> Handle(DeleteUserFeedbackCommand request, CancellationToken cancellationToken)
        {
            if(request == null || request.Id.IsEmpty())
            {
                return response.Process<string>(new BadRequestResponse($"Invalid request parameters."));
            }

            var exists = await repository.Feedback
                .Exists(f => f.Id.Equals(request.Id));

            if(!exists)
            {
                return response.Process<string>(new BadRequestResponse($"No feedback record found with Id: {request.Id}."));
            }

            await repository.Feedback
                .DeleteAsync(f => f.Id.Equals(request.Id));

            return response.Process<string>(new ApiOkResponse<string>("Feedback record successfully deleted."));
        }
    }
}
