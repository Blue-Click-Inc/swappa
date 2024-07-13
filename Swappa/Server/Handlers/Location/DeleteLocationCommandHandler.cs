using MediatR;
using Swappa.Data.Contracts;
using Swappa.Entities.Responses;
using Swappa.Server.Commands.Location;
using Swappa.Shared.DTOs;
using Swappa.Shared.Extensions;

namespace Swappa.Server.Handlers.Location
{
    public class DeleteLocationCommandHandler : IRequestHandler<DeleteLocationCommand, ResponseModel<string>>
    {
        private readonly IRepositoryManager repository;
        private readonly ApiResponseDto response;

        public DeleteLocationCommandHandler(IRepositoryManager repository,
            ApiResponseDto response)
        {
            this.repository = repository;
            this.response = response;
        }

        public async Task<ResponseModel<string>> Handle(DeleteLocationCommand request, CancellationToken cancellationToken)
        {
            if(request == null || request.EntityId.IsEmpty())
            {
                return response.Process<string>(new BadRequestResponse("Invalid request. Request parameters can not be null"));
            }

            var location = await repository.Location.FindOneAsync(l => l.EntityId.Equals(request.EntityId));
            if(location == null)
            {
                return response.Process<string>(new NotFoundResponse($"No location record found with the Id: {request.EntityId}"));
            }

            await repository.Location.DeleteAsync(l => l.Id.Equals(location.Id));
            return response.Process<string>(new ApiOkResponse<string>("Location record successfully deleted."));
        }
    }
}
