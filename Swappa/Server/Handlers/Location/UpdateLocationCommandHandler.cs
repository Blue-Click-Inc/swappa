using MediatR;
using Swappa.Data.Contracts;
using Swappa.Entities.Models;
using Swappa.Entities.Responses;
using Swappa.Server.Commands.Location;
using Swappa.Server.Validations.Location;
using Swappa.Shared.DTOs;
using Swappa.Shared.Extensions;

namespace Swappa.Server.Handlers.Location
{
    public class UpdateLocationCommandHandler : IRequestHandler<UpdateLocationCommand, ResponseModel<string>>
    {
        private readonly IRepositoryManager repository;
        private readonly ApiResponseDto response;

        public UpdateLocationCommandHandler(IRepositoryManager repository,
            ApiResponseDto response)
        {
            this.repository = repository;
            this.response = response;
        }

        public async Task<ResponseModel<string>> Handle(UpdateLocationCommand request, CancellationToken cancellationToken)
        {
            var validator = new LocationValidator();
            var validationResult = validator.Validate(request);
            if (!validationResult.IsValid)
            {
                return response.Process<string>(new BadRequestResponse(validationResult.Errors?.FirstOrDefault()?.ErrorMessage ?? string.Empty));
            }
            
            var country = await repository.Location.GetAsync(request.CountryId);
            if (country == null)
            {
                return response.Process<string>(new NotFoundResponse($"No country record found with the Id: {request.CountryId}"));
            }

            var state = await repository.Location.GetOneAsync(request.StateId);
            if (state == null)
            {
                return response.Process<string>(new NotFoundResponse($"No state record found with the Id: {request.StateId}"));
            }

            if (state.Country != country._Id)
            {
                return response.Process<string>(new BadRequestResponse($"{state.Name} is not in {country.Name}."));
            }

            var locationToUpdate = await repository.Location.GetByConditionAsync(l => l.EntityId.Equals(request.EntityId));
            if (locationToUpdate == null)
            {
                return response.Process<string>(new NotFoundResponse($"No location record found with the EntityId: {request.EntityId}"));
            }

            locationToUpdate.State = state.Name;
            locationToUpdate.Country = country.Name;
            locationToUpdate.City = request.City;
            locationToUpdate.PostalCode = request.PostalCode;
            locationToUpdate.UpdatedAt = DateTime.UtcNow;
            if(request.PostalCode != locationToUpdate.PostalCode)
            {
                // Update coordinate
            }

            await repository.Location.EditAsync(l => l.EntityId.Equals(request.EntityId), locationToUpdate);
            return response.Process<string>(new ApiOkResponse<string>("Location record successfully updated."));
        }
    }
}
