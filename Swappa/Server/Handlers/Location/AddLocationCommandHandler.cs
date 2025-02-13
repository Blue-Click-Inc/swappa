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
    public class AddLocationCommandHandler : IRequestHandler<AddLocationCommand, ResponseModel<string>>
    {
        private readonly IRepositoryManager repository;
        private readonly ApiResponseDto response;
        private readonly IExternalLocation location;

        public AddLocationCommandHandler(IRepositoryManager repository,
            ApiResponseDto response, IExternalLocation location)
        {
            this.repository = repository;
            this.response = response;
            this.location = location;
        }

        public async Task<ResponseModel<string>> Handle(AddLocationCommand request, CancellationToken cancellationToken)
        {
            var validator = new LocationValidator();
            var validationResult = validator.Validate(request);
            if (!validationResult.IsValid)
            {
                return response.Process<string>(new BadRequestResponse(validationResult.Errors?.FirstOrDefault()?.ErrorMessage ?? string.Empty));
            }

            var country = await location.GetCountry(request.CountryId.ToGuid());
            if(country == null)
            {
                return response.Process<string>(new NotFoundResponse($"No country record found with the Id: {request.CountryId}"));
            }

            var state = await location.GetState(request.CountryId.ToGuid(), request.StateId.ToGuid());
            if (state == null)
            {
                return response.Process<string>(new NotFoundResponse($"No state record found with the Id: {request.StateId}"));
            }

            if(!state.CountryId.Equals(country.Id))
            {
                return response.Process<string>(new BadRequestResponse($"{state.Name} is not in {country.Name}."));
            }

            if(await repository.Location.Exists(l => !l.IsDeprecated && l.EntityId.Equals(request.EntityId)))
            {
                return response.Process<string>(new BadRequestResponse($"Entity with Id: {request.EntityId} already has a location record."));
            }

            var data = MapLocation(request, country, state);
            await repository.Location.AddAsync(data);

            return response.Process<string>(new ApiOkResponse<string>("Location record successfully added."));
        }

        private EntityLocation MapLocation(AddLocationCommand request, CountryData country, StateData state)
        {
            return new EntityLocation
            {
                EntityId = request.EntityId,
                EntityType = request.EntityType,
                StateId = state.Id.ToString(),
                Country = country.Name,
                State = state.Name,
                CountryId = country.Id.ToString(),
                City = request.City,
                PostalCode = request.PostalCode,
                Coordinate = new Coordinate
                {
                    Latitude = state.Latitude,
                    Longitude = state.Longitude
                }
            };
        }
    }
}
