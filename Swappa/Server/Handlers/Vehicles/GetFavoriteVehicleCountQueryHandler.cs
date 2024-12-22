using MediatR;
using Swappa.Data.Contracts;
using Swappa.Entities.Responses;
using Swappa.Server.Queries.Vehicle;
using Swappa.Shared.DTOs;
using Swappa.Shared.Extensions;

namespace Swappa.Server.Handlers.Vehicles
{
    public class GetFavoriteVehicleCountQueryHandler : IRequestHandler<GetFavoriteVehicleCountQuery, ResponseModel<long>>
    {
        private readonly ApiResponseDto response;
        private readonly IRepositoryManager repository;

        public GetFavoriteVehicleCountQueryHandler(ApiResponseDto response, 
            IRepositoryManager repository)
        {
            this.response = response;
            this.repository = repository;
        }

        public async Task<ResponseModel<long>> Handle(GetFavoriteVehicleCountQuery request, CancellationToken cancellationToken)
        {
            if (request.UserId.IsEmpty())
            {
                response.Process<long>(new BadRequestResponse("Invalid logged in user id"));
            }

            var count = await repository.FavoriteVehicles.Count(f => f.UserId.Equals(request.UserId));
            return response.Process<long>(new ApiOkResponse<long>(count));
        }
    }
}
