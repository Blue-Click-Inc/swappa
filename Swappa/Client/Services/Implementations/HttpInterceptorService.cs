using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using Swappa.Client.Services.Interfaces;
using System.Net;
using System.Net.Http.Json;
using Toolbelt.Blazor;

namespace Swappa.Client.Services.Implementations
{
    public class HttpInterceptorService
    {
        private readonly NavigationManager navigation;
        private readonly ISharedService sharedService;
        private readonly IToastService toastService;

        public HttpInterceptorService(NavigationManager navigation, 
            ISharedService sharedService,
            IToastService toastService)
        {
            this.navigation = navigation;
            this.sharedService = sharedService;
            this.toastService = toastService;
        }

        public async Task<T?> Process<T>(HttpResponseMessage httpResponse) where T : class
        {
            T? result = null!;
            if (!httpResponse.IsSuccessStatusCode && 
                (httpResponse.StatusCode != HttpStatusCode.BadRequest || 
                httpResponse.StatusCode == HttpStatusCode.NotFound))
            {
                ProcessResponse(httpResponse.StatusCode);
            }
            else
            {
                result = await httpResponse.Content.ReadFromJsonAsync<T>();
            }

            return result;
        }
        private void ProcessResponse(HttpStatusCode statusCode)
        {
            string? message;
            switch (statusCode)
            {
                case HttpStatusCode.NotFound:
                    navigation.NavigateTo("/404");
                    message = "The requested resources not found";
                    break;
                case HttpStatusCode.Unauthorized:
                    message = "You are not authorized to access this page. Please login";
                    navigation.NavigateTo("/", true, true);
                    sharedService.LogoutAsync().RunSynchronously();
                    break;
                case HttpStatusCode.Forbidden:
                    message = "You do not have the permission to access this resources.";
                    navigation.NavigateTo("/unauthorized");
                    break;
                default:
                    message = "An error occurred. Please try again later";
                    break;
            }

            toastService.ShowError(message);
        }
    }
}
