using MediatR;
using Swappa.Data.Contracts;
using Swappa.Entities.Responses;
using Swappa.Server.Queries.Location;
using Swappa.Shared.DTOs;

namespace Swappa.Server.Handlers.Location
{
    public class GetCountriesQueryHandler : IRequestHandler<GetCountriesQuery, ResponseModel<List<CountryDataToReturnDto>>>
    {
        private readonly IRepositoryManager repository;
        private readonly ApiResponseDto response;

        public GetCountriesQueryHandler(IRepositoryManager repository,
            ApiResponseDto response)
        {
            this.repository = repository;
            this.response = response;
        }

        public async Task<ResponseModel<List<CountryDataToReturnDto>>> Handle(GetCountriesQuery request, CancellationToken cancellationToken)
        {
            var list = await repository.Location.GetAsync(request.PageNumber, request.PageSize);
            return response.Process<List<CountryDataToReturnDto>>(new ApiOkResponse<List<CountryDataToReturnDto>>(list));
        }
    }
}
