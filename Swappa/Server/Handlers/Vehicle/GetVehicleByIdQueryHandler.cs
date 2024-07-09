using AutoMapper;
using MediatR;
using Swappa.Data.Contracts;
using Swappa.Entities.Responses;
using Swappa.Server.Queries.Vehicle;
using Swappa.Shared.DTOs;

namespace Swappa.Server.Handlers.Vehicle
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

            var returnData = mapper.Map<VehicleToReturnDto>(vehicle);
            return response.Process<VehicleToReturnDto>(new ApiOkResponse<VehicleToReturnDto>(returnData));
        }
    }
}
