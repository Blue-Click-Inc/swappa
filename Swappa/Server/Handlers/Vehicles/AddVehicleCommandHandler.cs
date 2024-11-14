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
using Swappa.Shared.Extensions;

namespace Swappa.Server.Handlers.Vehicles
{
    public class AddVehicleCommandHandler : IRequestHandler<AddVehicleCommand, ResponseModel<string>>
    {
        private readonly IRepositoryManager repository;
        private readonly IMapper mapper;
        private readonly ApiResponseDto response;
        private readonly IServiceManager service;
        private readonly ILogger<AddVehicleCommandHandler> logger;

        public AddVehicleCommandHandler(IRepositoryManager repository,
            IMapper mapper, ApiResponseDto response, IServiceManager service, ILogger<AddVehicleCommandHandler> logger)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.response = response;
            this.service = service;
            this.logger = logger;
        }

        public async Task<ResponseModel<string>> Handle(AddVehicleCommand command, CancellationToken cancellationToken)
        {
            var validator = new VehicleValidator();
            var validationResult = validator.Validate(command.Request!);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors ?? new List<FluentValidation.Results.ValidationFailure>();
                logger.LogError($"Validation error while adding a vehicle: {string.Join(',', errors)}");
                return response.Process<string>(new BadRequestResponse(validationResult.Errors?.FirstOrDefault()?.ErrorMessage ?? string.Empty));
            }

            var userId = repository.Common.GetUserIdAsGuid();
            if (userId.IsEmpty())
            {
                return response.Process<string>(new BadRequestResponse($"Invalid looged in user id"));
            }

            var vehicleToAdd = mapper.Map<Vehicle>(command.Request);
            vehicleToAdd.UserId = userId;
            await repository.Vehicle.AddAsync(vehicleToAdd);

            await Task.Run(async () =>
            {
                var userLocation = await repository.Location.FindOneAsync(_ => _.EntityId.Equals(userId));
                if (userLocation.IsNotNull())
                {
                    var location = new EntityLocation
                    {
                        EntityId = vehicleToAdd.Id,
                        EntityType = EntityType.Vehicle,
                        City = userLocation.City,
                        Country = userLocation.Country,
                        State = userLocation.State,
                        Coordinate = userLocation.Coordinate,
                        StateId = userLocation.StateId,
                        CountryId = userLocation.CountryId,
                        PostalCode = userLocation.PostalCode
                    };
                    await repository.Location.AddAsync(location);
                }

                if (command.Request.Images.IsNotNullOrEmpty())
                {
                    await service.Cloudinary.UploadVehicleImages(vehicleToAdd.Id, command.Request.Images, true);
                }
            }, cancellationToken);
            return response.Process<string>(new ApiOkResponse<string>("Vehicle successfully added"));
        }
    }
}