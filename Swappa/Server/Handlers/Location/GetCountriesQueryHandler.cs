using MediatR;
using Swappa.Data.Contracts;
using Swappa.Entities.Responses;
using Swappa.Server.Queries.Location;
using Swappa.Shared.DTOs;
using Swappa.Shared.Interface;

namespace Swappa.Server.Handlers.Location
{
    public class GetCountriesQueryHandler : IRequestHandler<GetCountriesQuery, ResponseModel<List<CountryData>>>
    {
        private readonly IRepositoryManager repository;
        private readonly ApiResponseDto response;
        private readonly IExternalLocation location;

        public GetCountriesQueryHandler(IRepositoryManager repository,
            ApiResponseDto response, IExternalLocation location)
        {
            this.repository = repository;
            this.response = response;
            this.location = location;
        }

        public async Task<ResponseModel<List<CountryData>>> Handle(GetCountriesQuery request, CancellationToken cancellationToken)
        {
            var list = await location.GetCountriesAsync();
            return response.Process<List<CountryData>>(new ApiOkResponse<List<CountryData>>(list.Data));
        }
    }
}
