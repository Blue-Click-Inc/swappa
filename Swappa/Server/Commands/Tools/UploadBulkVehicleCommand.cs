using MediatR;
using Swappa.Shared.DTOs;

namespace Swappa.Server.Commands.Tools
{
    public class UploadBulkVehicleCommand : IRequest<ResponseModel<string>>
    {
        public IFormFile File { get; set; }
    }
}
