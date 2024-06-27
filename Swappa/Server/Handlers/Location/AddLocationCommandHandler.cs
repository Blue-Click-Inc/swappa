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
    public class AddLocationCommandHandler : IRequestHandler<AddLocationCommand, ResponseModel<string>>
    {
        private readonly IRepositoryManager repository;
        private readonly ApiResponseDto response;

        public AddLocationCommandHandler(IRepositoryManager repository,
            ApiResponseDto response)
        {
            this.repository = repository;
            this.response = response;
        }

        public async Task<ResponseModel<string>> Handle(AddLocationCommand request, CancellationToken cancellationToken)
        {
            var validator = new LocationValidator();
            var validationResult = validator.Validate(request);
            if (!validationResult.IsValid)
            {
                return response.Process<string>(new BadRequestResponse(validationResult.Errors?.FirstOrDefault()?.ErrorMessage ?? string.Empty));
            }

            var country = await repository.Location.GetAsync(request.CountryId);
            if(country == null)
            {
                return response.Process<string>(new NotFoundResponse($"No country record found with the Id: {request.CountryId}"));
            }

            var state = await repository.Location.GetOneAsync(request.StateId);
            if (state == null)
            {
                return response.Process<string>(new NotFoundResponse($"No state record found with the Id: {request.StateId}"));
            }

            if(state.Country != country._Id)
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

        private EntityLocation MapLocation(AddLocationCommand request, CountryDataToReturnDto country, StateDataToReturnDto state)
        {
            return new EntityLocation
            {
                EntityId = request.EntityId,
                EntityType = request.EntityType,
                StateId = state._Id,
                CountryId = country._Id,
                City = request.City,
                PostalCode = request.PostalCode
            };
        }
    }
}
