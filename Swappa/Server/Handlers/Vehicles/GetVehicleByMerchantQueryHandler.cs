using AutoMapper;
using MediatR;
using Swappa.Data.Contracts;
using Swappa.Entities.Models;
using Swappa.Entities.Responses;
using Swappa.Server.Extensions;
using Swappa.Server.Queries.Vehicle;
using Swappa.Shared.DTOs;
using Swappa.Shared.Extensions;

namespace Swappa.Server.Handlers.Vehicles
{
    public class GetVehicleByMerchantQueryHandler : IRequestHandler<GetVehicleByMerchantQuery, ResponseModel<PaginatedListDto<VehicleToReturnDto>>>
    {
        private readonly ApiResponseDto response;
        private readonly IRepositoryManager repository;
        private readonly IMapper mapper;

        public GetVehicleByMerchantQueryHandler(ApiResponseDto response,
            IRepositoryManager repository, IMapper mapper)
        {
            this.response = response;
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<ResponseModel<PaginatedListDto<VehicleToReturnDto>>> Handle(GetVehicleByMerchantQuery request, CancellationToken cancellationToken)
        {
            if (request.MerchantId.IsEmpty())
            {
                return response.Process<PaginatedListDto<VehicleToReturnDto>>(
                    new ApiOkResponse<PaginatedListDto<VehicleToReturnDto>>(new PaginatedListDto<VehicleToReturnDto>()));
            }

            var query = repository.Vehicle.FindAsQueryable(v => !v.IsDeprecated && v.UserId.Equals(request.MerchantId))
                .OrderByDescending(v => v.CreatedAt)
                .Search(request.Query.SearchTerm);

            var pagedList = await Task.Run(() => PagedList<Vehicle>.ToPagedList(query, request.Query.PageNumber, request.Query.PageSize));

            var data = mapper.Map<List<VehicleToReturnDto>>(pagedList);
            var pagedData = PaginatedListDto<VehicleToReturnDto>.Paginate(data, pagedList.MetaData);
            return response.Process<PaginatedListDto<VehicleToReturnDto>>(new ApiOkResponse<PaginatedListDto<VehicleToReturnDto>>(pagedData));
        }
    }
}