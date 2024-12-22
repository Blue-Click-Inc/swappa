using MediatR;
using Swappa.Data.Contracts;
using Swappa.Entities.Models;
using Swappa.Entities.Responses;
using Swappa.Server.Commands.Vehicle;
using Swappa.Shared.DTOs;
using Swappa.Shared.Extensions;

namespace Swappa.Server.Handlers.Vehicles
{
    public class ToggleFavoriteVehicleCommandHandler : IRequestHandler<ToggleFavoriteVehicleCommand, ResponseModel<FavoriteVehicleResponseDto>>
    {
        private readonly ApiResponseDto response;
        private readonly IRepositoryManager repository;

        public ToggleFavoriteVehicleCommandHandler(ApiResponseDto response,
            IRepositoryManager repository)
        {
            this.response = response;
            this.repository = repository;
        }

        public async Task<ResponseModel<FavoriteVehicleResponseDto>> Handle(ToggleFavoriteVehicleCommand request, CancellationToken cancellationToken)
        {
            var loggedInUserId = repository.Common.GetUserIdAsGuid();
            if(loggedInUserId.IsEmpty())
            {
                response.Process<long>(new BadRequestResponse("Invalid logged in user id"));
            }

            var vehicle = await repository.Vehicle.FindAsync(v => v.Id.Equals(request.VehicleId));
            if (vehicle.IsNull())
            {
                response.Process<long>(new NotFoundResponse("Vehicle not found"));
            }

            var exists = await repository.FavoriteVehicles
                .Exists(f => f.UserId.Equals(loggedInUserId) && f.VehicleId.Equals(request.VehicleId));

            var isFavorite = false;
            if (exists)
            {
                await repository.FavoriteVehicles
                    .DeleteAsync(f => f.UserId.Equals(loggedInUserId) && f.VehicleId.Equals(request.VehicleId));

                vehicle.Favorited -= 1;
                await repository.Vehicle
                    .EditAsync(v => v.Id.Equals(request.VehicleId), vehicle);
            }
            else
            {
                await repository.FavoriteVehicles.AddAsync(new FavoriteVehicles
                {
                    UserId = loggedInUserId,
                    VehicleId = request.VehicleId
                });

                vehicle.Favorited += 1;
                await repository.Vehicle
                    .EditAsync(v => v.Id.Equals(request.VehicleId), vehicle);

                isFavorite = true;
            }

            var count = await repository.FavoriteVehicles.Count(f => f.UserId.Equals(loggedInUserId));
            return response
                .Process<FavoriteVehicleResponseDto>(new ApiOkResponse<FavoriteVehicleResponseDto>(new FavoriteVehicleResponseDto
                {
                    Favorites = vehicle.Favorited,
                    AllFavorites = count,
                    IsFavorite = isFavorite
                }));
        }
    }
}
