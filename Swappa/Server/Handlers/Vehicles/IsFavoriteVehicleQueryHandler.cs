using MediatR;
using Swappa.Data.Contracts;
using Swappa.Entities.Responses;
using Swappa.Server.Queries.Vehicle;
using Swappa.Shared.DTOs;
using Swappa.Shared.Extensions;

namespace Swappa.Server.Handlers.Vehicles
{
    public class IsFavoriteVehicleQueryHandler : IRequestHandler<IsFavoriteVehicleQuery, ResponseModel<bool>>
    {
        private readonly ApiResponseDto response;
        private readonly IRepositoryManager repository;

        public IsFavoriteVehicleQueryHandler(ApiResponseDto response, 
            IRepositoryManager repository)
        {
            this.response = response;
            this.repository = repository;
        }

        public async Task<ResponseModel<bool>> Handle(IsFavoriteVehicleQuery request, CancellationToken cancellationToken)
        {
            if (request.Id.IsEmpty())
            {
                return response.Process<bool>(new BadRequestResponse("Invalid vehicle Id"));
            }

            var loggedInUserId = repository.Common.GetUserIdAsGuid();
            if (loggedInUserId.IsEmpty())
            {
                return response.Process<bool>(new BadRequestResponse("Invalid logged in User Id"));
            }

            var exists = await repository.FavoriteVehicles
                .Exists(v => v.VehicleId.Equals(request.Id) && v.UserId.Equals(loggedInUserId));

            return response.Process<bool>(new ApiOkResponse<bool>(exists));
        }
    }
}
