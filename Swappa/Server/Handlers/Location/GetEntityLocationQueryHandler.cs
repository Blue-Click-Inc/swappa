using AutoMapper;
using MediatR;
using Swappa.Data.Contracts;
using Swappa.Entities.Responses;
using Swappa.Server.Queries.Location;
using Swappa.Shared.DTOs;
using Swappa.Shared.Extensions;

namespace Swappa.Server.Handlers.Location
{
    public class GetEntityLocationQueryHandler : IRequestHandler<GetEntityLocationQuery, ResponseModel<BaseLocationDto>>
    {
        private readonly IRepositoryManager repository;
        private readonly ApiResponseDto response;
        private readonly IMapper mapper;

        public GetEntityLocationQueryHandler(IRepositoryManager repository,
            ApiResponseDto response, IMapper mapper)
        {
            this.repository = repository;
            this.response = response;
            this.mapper = mapper;
        }

        public async Task<ResponseModel<BaseLocationDto>> Handle(GetEntityLocationQuery request, CancellationToken cancellationToken)
        {
            if(request == null || request.EntityId.IsEmpty())
            {
                return response.Process<BaseLocationDto>(new BadRequestResponse("Invalid request parameters!"));
            }

            var location = await repository.Location.FindOneAsync(l => l.EntityId.Equals(request.EntityId));
            if (location == null)
            {
                return response.Process<BaseLocationDto>(new NotFoundResponse($"No location record with the Id: {request.EntityId}"));
            }

            var data = mapper.Map<BaseLocationDto>(location);
            return response.Process<BaseLocationDto>(new ApiOkResponse<BaseLocationDto>(data));
        }
    }
}
