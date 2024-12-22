using AutoMapper;
using MediatR;
using Swappa.Data.Contracts;
using Swappa.Entities.Models;
using Swappa.Entities.Responses;
using Swappa.Server.Queries.Vehicle;
using Swappa.Shared.DTOs;
using Swappa.Shared.Extensions;

namespace Swappa.Server.Handler.Vehicles
{
    public class GetVehicleByIdQueryHandler : IRequestHandler<GetVehicleByIdQuery, ResponseModel<VehicleToReturnDto>>
    {
        private readonly IRepositoryManager repository;
        private readonly ApiResponseDto response;
        private readonly IMapper mapper;

        public GetVehicleByIdQueryHandler(IRepositoryManager repository,
            ApiResponseDto response, IMapper mapper)
        {
            this.repository = repository;
            this.response = response;
            this.mapper = mapper;
        }

        public async Task<ResponseModel<VehicleToReturnDto>> Handle(GetVehicleByIdQuery request, CancellationToken cancellationToken)
        {
            var vehicle = await repository.Vehicle.FindAsync(v => v.Id == request.Id && !v.IsDeprecated);
            if(vehicle == null)
            {
                return response.Process<VehicleToReturnDto>(new BadRequestResponse($"No record vehicle found for the Id: {request.Id}"));
            }

            var images = await repository.Image.FindManyAsync(i => i.VehicleId == request.Id);
            if(images != null)
            {
                vehicle.Images = images;
            }

            await Task.Run(async () =>
            {
                var loggedInUserId = repository.Common.GetUserIdAsGuid();
                if (loggedInUserId.IsNotEmpty())
                {
                    var alreadyViewed = await repository
                    .VehicleViews.Exists(x => x.VehicleId.Equals(request.Id) && x.UserId.Equals(loggedInUserId));
                    if (!alreadyViewed && !loggedInUserId.Equals(vehicle.UserId))
                    {
                        await repository.VehicleViews.AddAsync(new VehicleViews
                        {
                            VehicleId = vehicle.Id,
                            UserId = loggedInUserId,
                        });

                        vehicle.Views++;
                        await repository.Vehicle.EditAsync(x => x.Id.Equals(vehicle.Id), vehicle);
                    }

                    var isUserFavorite = await repository.FavoriteVehicles
                        .Exists(v => v.VehicleId.Equals(request.Id) && v.UserId.Equals(loggedInUserId));

                    vehicle.IsFavorite = isUserFavorite;
                }
            }, CancellationToken.None);

            var returnData = mapper.Map<VehicleToReturnDto>(vehicle);
            return response.Process<VehicleToReturnDto>(new ApiOkResponse<VehicleToReturnDto>(returnData));
        }
    }
}
