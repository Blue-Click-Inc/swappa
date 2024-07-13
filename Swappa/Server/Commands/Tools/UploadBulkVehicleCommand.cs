using MediatR;
using Microsoft.AspNetCore.Components.Forms;
using Swappa.Shared.DTOs;

namespace Swappa.Server.Commands.Tools
{
    public class UploadBulkVehicleCommand : IRequest<ResponseModel<string>>
    {
        public IFormFile? File { get; set; }
    }
}
