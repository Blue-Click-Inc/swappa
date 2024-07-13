using Swappa.Client.Services.Interfaces;
using Swappa.Shared.DTOs;

namespace Swappa.Client.Services.Implementations
{
    public class ToolsService : IToolsService
    {
        private readonly HttpClient httpClient;
        private readonly HttpInterceptorService httpInterceptor;

        public ToolsService(HttpClient httpClient, HttpInterceptorService httpInterceptor)
        {
            this.httpClient = httpClient;
            this.httpInterceptor = httpInterceptor;
        }

        public async Task<ResponseModel<string>?> UploadBulkVehicle(MultipartFormDataContent? content)
        {
            var response = await httpClient.PostAsync("tools/vehicle-upload-bulk", content);
            return await httpInterceptor.Process<ResponseModel<string>>(response);
        }
    }
}
