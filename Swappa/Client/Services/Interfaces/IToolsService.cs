using Swappa.Shared.DTOs;

namespace Swappa.Client.Services.Interfaces
{
    public interface IToolsService
    {
        Task<ResponseModel<string>?> UploadBulkVehicle(MultipartFormDataContent? content);
    }
}
