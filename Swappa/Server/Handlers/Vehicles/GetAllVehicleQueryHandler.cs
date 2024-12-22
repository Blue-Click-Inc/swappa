using AutoMapper;
using MediatR;
using Swappa.Data.Contracts;
using Swappa.Entities.Models;
using Swappa.Entities.Responses;
using Swappa.Server.Extensions;
using Swappa.Server.Queries.Vehicle;
using Swappa.Shared.DTOs;

namespace Swappa.Server.Handlers.Vehicles
{
    public class GetAllVehicleQueryHandler : IRequestHandler<GetAllVehiclesQuery, ResponseModel<PaginatedListDto<VehicleToReturnDto>>>
    {
        private readonly IRepositoryManager repository;
        private readonly IMapper mapper;
        private readonly ApiResponseDto response;

        public GetAllVehicleQueryHandler(IRepositoryManager repository,
            IMapper mapper, ApiResponseDto response)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.response = response;
        }

        public async Task<ResponseModel<PaginatedListDto<VehicleToReturnDto>>> Handle(GetAllVehiclesQuery request, CancellationToken cancellationToken)
        {
            var query = repository.Vehicle.FindAsQueryable(v => !v.IsDeprecated && !request.IncludeSold ? !v.IsSold : true)
                .OrderByDescending(v => v.CreatedAt)
                .Search(request.SearchTerm)
                .Filter(request);

            var pagedList = PagedList<Vehicle>.ToPagedList(query, request.PageNumber, request.PageSize);
            var vehicleIds = pagedList.Select(v => v.Id).ToList();

            var locations = (await repository.Location.FindManyAsync(l => vehicleIds.Contains(l.EntityId)))
                .ToDictionary(l => l.EntityId);

            var images = (await repository.Image.FindManyAsync(i => vehicleIds.Contains(i.VehicleId)))
                .GroupBy(i => i.VehicleId)
                .ToDictionary(i => i.Key, i => i.ToList());

            pagedList.MapLocations(locations)
                .MapImages(images);
            
            var data = mapper.Map<List<VehicleToReturnDto>>(pagedList);
            var pagedData = PaginatedListDto<VehicleToReturnDto>.Paginate(data, pagedList.MetaData);
            return response.Process<PaginatedListDto<VehicleToReturnDto>>(new ApiOkResponse<PaginatedListDto<VehicleToReturnDto>>(pagedData));
        }
    }
}
