using MediatR;
using Swappa.Data.Contracts;
using Swappa.Entities.Models;
using Swappa.Entities.Responses;
using Swappa.Server.Commands.Location;
using Swappa.Server.Validations.Location;
using Swappa.Shared.DTOs;
using Swappa.Shared.Extensions;
using Swappa.Shared.Interface;

namespace Swappa.Server.Handlers.Location
{
    public class UpdateLocationCommandHandler : IRequestHandler<UpdateLocationCommand, ResponseModel<string>>
    {
        private readonly IRepositoryManager repository;
        private readonly ApiResponseDto response;
        private readonly IExternalLocation location;

        public UpdateLocationCommandHandler(IRepositoryManager repository,
            ApiResponseDto response, IExternalLocation location)
        {
            this.repository = repository;
            this.response = response;
            this.location = location;
        }

        public async Task<ResponseModel<string>> Handle(UpdateLocationCommand request, CancellationToken cancellationToken)
        {
            var validator = new LocationValidator();
            var validationResult = validator.Validate(request);
            if (!validationResult.IsValid)
            {
                return response.Process<string>(new BadRequestResponse(validationResult.Errors?.FirstOrDefault()?.ErrorMessage ?? string.Empty));
            }
            
            var country = await location.GetCountry(request.CountryId.ToGuid());
            if (country == null)
            {
                return response.Process<string>(new NotFoundResponse($"No country record found with the Id: {request.CountryId}"));
            }

            var state = await location.GetState(request.CountryId.ToGuid(), request.StateId.ToGuid());
            if (state == null)
            {
                return response.Process<string>(new NotFoundResponse($"No state record found with the Id: {request.StateId}"));
            }

            if (state.CountryId != country.Id)
            {
                return response.Process<string>(new BadRequestResponse($"{state.Name} is not in {country.Name}."));
            }

            var locationToUpdate = await repository.Location.FindOneAsync(l => l.EntityId.Equals(request.EntityId));
            if (locationToUpdate == null)
            {
                return response.Process<string>(new NotFoundResponse($"No location record found with the EntityId: {request.EntityId}"));
            }

            locationToUpdate.StateId = state.Id.ToString();
            locationToUpdate.CountryId = country.Id.ToString();
            locationToUpdate.City = request.City;
            locationToUpdate.Country = country.Name;
            locationToUpdate.State = state.Name;
            locationToUpdate.PostalCode = request.PostalCode;
            locationToUpdate.UpdatedAt = DateTime.UtcNow;
            if(request.PostalCode != locationToUpdate.PostalCode)
            {
                locationToUpdate.Coordinate.Latitude = state.Latitude;
                locationToUpdate.Coordinate.Longitude = state.Longitude;
            }

            await repository.Location.EditAsync(l => l.EntityId.Equals(request.EntityId), locationToUpdate);
            return response.Process<string>(new ApiOkResponse<string>("Location record successfully updated."));
        }
    }
}
