using MediatR;
using Swappa.Shared.DTOs;

namespace Swappa.Server.Queries.Vehicle
{
    public class IsFavoriteVehicleQuery : IRequest<ResponseModel<bool>>
    {
        public Guid Id { get; set; }
    }
}
