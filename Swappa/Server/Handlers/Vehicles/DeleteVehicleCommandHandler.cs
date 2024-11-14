using MediatR;
using Mongo.Common.MongoDB;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using Swappa.Entities.Enums;
using Swappa.Entities.Models;
using Swappa.Server.Commands.Vehicle;
using Swappa.Shared.DTOs;
using System.Drawing;
using Swappa.Data.Contracts;
using Swappa.Shared.Extensions;
using Microsoft.IdentityModel.Tokens;
using Swappa.Entities.Responses;

namespace Swappa.Server.Handlers.Vehicles
{
    public class DeleteVehicleCommandHandler : IRequestHandler<DeleteVehicleCommand, ResponseModel<string>>
    {
        private readonly ApiResponseDto response;
        private readonly IRepositoryManager repository;
        private readonly ILogger<DeleteVehicleCommandHandler> logger;

        public DeleteVehicleCommandHandler(ApiResponseDto response,
            IRepositoryManager repository, ILogger<DeleteVehicleCommandHandler> logger) 
        {
            this.response = response;
            this.repository = repository;
            this.logger = logger;
        }

        public async Task<ResponseModel<string>> Handle(DeleteVehicleCommand request, CancellationToken cancellationToken)
        {
            if (request.Id.IsEmpty())
            {
                return response.Process<string>(new BadRequestResponse($"Invalid vehicle Id: {request.Id}"));
            }

            var userId = repository.Common.GetUserIdAsGuid();
            if(userId.IsEmpty())
            {
                logger.LogError($"Invalid logged in user id: {userId}");
                return response.Process<string>(new BadRequestResponse($"Something went wrong. Could not determine the logged in user claims"));
            }

            var vehicleToDelete = await repository.Vehicle.FindAsync(v => v.Id.Equals(request.Id));
            if (vehicleToDelete.IsNull())
            {
                return response.Process<string>(new NotFoundResponse($"Could not find a vehicle with the id: {request.Id}"));
            }

            if(!vehicleToDelete.UserId.Equals(userId))
            {
                return response.Process<string>(new BadRequestResponse("You are only allowed to delete a vehicle you added"));
            }

            vehicleToDelete.IsDeprecated = true;
            vehicleToDelete.UpdatedAt = DateTime.UtcNow;
            await repository.Vehicle.EditAsync(x => x.Id.Equals(request.Id), vehicleToDelete);
            var location = await repository.Location.FindOneAsync(l => l.EntityId.Equals(request.Id));
            if (location != null)
            {
                location.IsDeprecated = true;
                location.UpdatedAt = DateTime.UtcNow;
                await repository.Location.EditAsync(l => l.Id.Equals(location.Id), location);
            }
            return response.Process<string>(new ApiOkResponse<string>("Vehicle record successfully deleted"));
        }
    }
}
