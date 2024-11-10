using AutoMapper;
using MediatR;
using Swappa.Data.Contracts;
using Swappa.Entities.Responses;
using Swappa.Server.Commands.Vehicle;
using Swappa.Shared.DTOs;
using Swappa.Shared.Extensions;

namespace Swappa.Server.Handlers.Vehicles
{
    public class UpdateVehicleCommandHandler : IRequestHandler<UpdateVehicleCommand, ResponseModel<string>>
    {
        private readonly ApiResponseDto response;
        private readonly IRepositoryManager repository;
        private readonly IMapper mapper;
        private readonly ILogger<UpdateVehicleCommandHandler> logger;

        public UpdateVehicleCommandHandler(ApiResponseDto response,
            IRepositoryManager repository, IMapper mapper,
            ILogger<UpdateVehicleCommandHandler> logger)
        {
            this.response = response;
            this.repository = repository;
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task<ResponseModel<string>> Handle(UpdateVehicleCommand request, CancellationToken cancellationToken)
        {
            if (request.Id.IsEmpty())
            {
                return response.Process<string>(new BadRequestResponse($"Invalid vehicle Id: {request.Id}"));
            }

            var userId = repository.Common.GetUserIdAsGuid();
            if (userId.IsEmpty())
            {
                logger.LogError($"Invalid logged in user id: {userId}");
                return response.Process<string>(new BadRequestResponse($"Something went wrong. Could not determine the logged in user claims"));
            }

            var vehicleToUpdate = await repository.Vehicle.FindAsync(v => v.Id.Equals(request.Id));
            if (vehicleToUpdate.IsNull())
            {
                return response.Process<string>(new NotFoundResponse($"Could not find a vehicle with the id: {request.Id}"));
            }

            if (!vehicleToUpdate.UserId.Equals(userId))
            {
                return response.Process<string>(new BadRequestResponse("You are only allowed to update a vehicle you added"));
            }

            vehicleToUpdate = mapper.Map(request.Request, vehicleToUpdate);
            vehicleToUpdate.UpdatedAt = DateTime.UtcNow;
            await repository.Vehicle.EditAsync(x => x.Id.Equals(request.Id), vehicleToUpdate);
            
            return response.Process<string>(new ApiOkResponse<string>("Vehicle record successfully updated."));
        }
    }
}
