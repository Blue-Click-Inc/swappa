using MediatR;
using Swappa.Data.Contracts;
using Swappa.Entities.Responses;
using Swappa.Server.Queries.Vehicle;
using Swappa.Shared.DTOs;

namespace Swappa.Server.Handlers.Vehicles
{
    public class VehicleDashboardQueryHandler : IRequestHandler<VehicleDashboardQuery, ResponseModel<VehicleDashboardDto>>
    {
        private readonly IRepositoryManager repository;
        private readonly ApiResponseDto response;

        public VehicleDashboardQueryHandler(IRepositoryManager repository,
            ApiResponseDto response)
        {
            this.repository = repository;
            this.response = response;
        }

        public async Task<ResponseModel<VehicleDashboardDto>> Handle(VehicleDashboardQuery request, CancellationToken cancellationToken)
        {
            var query = repository.Vehicle
                .FindAsQueryable(v => !v.IsDeprecated);

            var total = await Task.Run(() => query.Count());
            var byEngine = await Task.Run(() => query
                .GroupBy(v => v.Engine).ToDictionary(v => v.Key, v => v.Count()));

            var byTransmission = await Task.Run(() => query
                .GroupBy(v => v.Transmission).ToDictionary(v => v.Key, v => v.Count()));

            var byDriveTrain = await Task.Run(() => query
                .GroupBy(v => v.DriveTrain).ToDictionary(v => v.Key, v => v.Count()));

            return response.Process<VehicleDashboardDto>(
                new ApiOkResponse<VehicleDashboardDto>(new VehicleDashboardDto
                {
                    TotalCount = total,
                    DriveTrain = byDriveTrain,
                    Transmission = byTransmission,
                    Engine = byEngine
                }));
        }
    }
}
