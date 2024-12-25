using AutoMapper;
using MediatR;
using Swappa.Data.Contracts;
using Swappa.Entities.Models;
using Swappa.Entities.Responses;
using Swappa.Server.Extensions;
using Swappa.Server.Queries.Vehicle;
using Swappa.Shared.DTOs;
using Swappa.Shared.Extensions;
using System.Linq;

namespace Swappa.Server.Handlers.Vehicles
{
    public class GetFavoriteVehiclesQueryHandler : IRequestHandler<GetFavoriteVehiclesQuery, ResponseModel<PaginatedListDto<VehicleToReturnDto>>>
    {
        private readonly ApiResponseDto response;
        private readonly IRepositoryManager repository;
        private readonly IMapper mapper;

        public GetFavoriteVehiclesQueryHandler(ApiResponseDto response,
            IRepositoryManager repository, IMapper mapper)
        {
            this.response = response;
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<ResponseModel<PaginatedListDto<VehicleToReturnDto>>> Handle(GetFavoriteVehiclesQuery request, CancellationToken cancellationToken)
        {
            if (request.UserId.IsEmpty())
            {
                return response.Process<PaginatedListDto<VehicleToReturnDto>>(
                    new ApiOkResponse<PaginatedListDto<VehicleToReturnDto>>(new PaginatedListDto<VehicleToReturnDto>()));
            }

            var favoriteVehicles = await Task.Run(() => repository.FavoriteVehicles
                .FindAsQueryable(v => v.UserId.Equals(request.UserId))
                .Select(v => v.VehicleId)
                .ToList());

            var query = repository.Vehicle.FindAsQueryable(v => !v.IsDeprecated && favoriteVehicles.Contains(v.Id))
                .OrderByDescending(v => v.CreatedAt)
                .Search(request.Request.SearchTerm);

            var pagedList = PagedList<Vehicle>.ToPagedList(query, request.Request.PageNumber, request.Request.PageSize);
            var vehicleIds = pagedList.Select(v => v.Id).ToList();

            var data = mapper.Map<List<VehicleToReturnDto>>(pagedList);
            var pagedData = PaginatedListDto<VehicleToReturnDto>.Paginate(data, pagedList.MetaData);
            return response.Process<PaginatedListDto<VehicleToReturnDto>>(new ApiOkResponse<PaginatedListDto<VehicleToReturnDto>>(pagedData));
        }
    }
}
