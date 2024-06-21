using MediatR;
using Swappa.Data.Contracts;
using Swappa.Entities.Responses;
using Swappa.Server.Queries.Location;
using Swappa.Shared.DTOs;

namespace Swappa.Server.Handlers.Location
{
    public class GetStatesQueryHandler : IRequestHandler<GetStatesQuery, ResponseModel<List<StateDataToReturnDto>>>
    {
        private readonly IRepositoryManager repository;
        private readonly ApiResponseDto response;

        public GetStatesQueryHandler(IRepositoryManager repository,
            ApiResponseDto response)
        {
            this.repository = repository;
            this.response = response;
        }

        public async Task<ResponseModel<List<StateDataToReturnDto>>> Handle(GetStatesQuery request, CancellationToken cancellationToken)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.CountryId))
            {
                return response.Process<List<StateDataToReturnDto>>(new BadRequestResponse("Invalid parameter. CountryId can not be null!"));
            }

            var list = await repository.Location.GetManyAsync(request.CountryId);
            return response.Process<List<StateDataToReturnDto>>(new ApiOkResponse<List<StateDataToReturnDto>>(list));
        }
    }
}
