using MediatR;
using Swappa.Data.Contracts;
using Swappa.Entities.Responses;
using Swappa.Server.Queries.Location;
using Swappa.Shared.DTOs;
using Swappa.Shared.Extensions;
using Swappa.Shared.Interface;

namespace Swappa.Server.Handlers.Location
{
    public class GetStatesQueryHandler : IRequestHandler<GetStatesQuery, ResponseModel<List<StateData>>>
    {
        private readonly IRepositoryManager repository;
        private readonly ApiResponseDto response;
        private readonly IExternalLocation location;

        public GetStatesQueryHandler(IRepositoryManager repository,
            ApiResponseDto response, IExternalLocation location)
        {
            this.repository = repository;
            this.response = response;
            this.location = location;
        }

        public async Task<ResponseModel<List<StateData>>> Handle(GetStatesQuery request, CancellationToken cancellationToken)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.CountryId))
            {
                return response.Process<List<StateData>>(new BadRequestResponse("Invalid parameter. CountryId can not be null!"));
            }

            var list = await location.GetStatesAsync(request.CountryId.ToGuid());
            return response.Process<List<StateData>>(new ApiOkResponse<List<StateData>>(list));
        }
    }
}
