using MediatR;
using Swappa.Shared.DTOs;

namespace Swappa.Server.Queries.Vehicle
{
    public sealed class GetVehicleByMerchantQuery : IRequest<ResponseModel<PaginatedListDto<VehicleToReturnDto>>>
    {
        public Guid MerchantId { get; set; }
        public VehicleQueryDto Query { get; set; } = new();
    }
}
