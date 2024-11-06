using AutoMapper;
using Hangfire;
using MediatR;
using Swappa.Data.Contracts;
using Swappa.Data.Services.Interfaces;
using Swappa.Entities.Enums;
using Swappa.Entities.Models;
using Swappa.Entities.Responses;
using Swappa.Server.Commands.Vehicle;
using Swappa.Server.Validations.Vehicle;
using Swappa.Shared.DTOs;

namespace Swappa.Server.Handlers.Vehicles
{
    public class AddVehicleCommandHandler : IRequestHandler<AddVehicleCommand, ResponseModel<string>>
    {
        private readonly IRepositoryManager repository;
        private readonly IMapper mapper;
        private readonly ApiResponseDto response;
        private readonly IServiceManager service;

        public AddVehicleCommandHandler(IRepositoryManager repository,
            IMapper mapper, ApiResponseDto response, IServiceManager service)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.response = response;
            this.service = service;
        }

        public async Task<ResponseModel<string>> Handle(AddVehicleCommand request, CancellationToken cancellationToken)
        {
            var validator = new VehicleValidator();
            var validationResult = validator.Validate(request);
            if (!validationResult.IsValid)
            {
                return response.Process<string>(new BadRequestResponse(validationResult.Errors?.FirstOrDefault()?.ErrorMessage ?? string.Empty));
            }

            var userId = repository.Common.GetUserIdAsGuid();

            var vehicleToAdd = mapper.Map<Vehicle>(request);
            vehicleToAdd.UserId = userId;

            var userLocation = await repository.Location.FindOneAsync(_ => _.EntityId.Equals(userId));
            var location = mapper.Map<EntityLocation>(userLocation);
            location.EntityId = vehicleToAdd.Id;
            location.EntityType = EntityType.Vehicle;

            await repository.Vehicle.AddAsync(vehicleToAdd);
            await repository.Location.AddAsync(location);
            BackgroundJob.Enqueue<ICloudinaryService>(x => x.UploadVehicleImages(vehicleToAdd.Id, request.Images, true));

            return response.Process<string>(new ApiOkResponse<string>("Vehicle successfully added"));
        }
    }
}
