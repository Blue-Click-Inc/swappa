using Blazored.LocalStorage;
using Blazored.Modal;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using Swappa.Client.Services.Interfaces;

namespace Swappa.Client.Services.Implementations
{
    public class SharedService : ISharedService
    {
        private readonly ILocalStorageService localStorage;
        private readonly AuthenticationStateProvider stateProvider;
        private readonly NavigationManager navigationManager;
        private readonly IJSRuntime jSRuntime;

        public SharedService(ILocalStorageService localStorage, 
            AuthenticationStateProvider stateProvider, 
            NavigationManager navigationManager,
            IJSRuntime jSRuntime)
        {
            this.localStorage = localStorage;
            this.stateProvider = stateProvider;
            this.navigationManager = navigationManager;
            this.jSRuntime = jSRuntime;
        }

        public async Task LogoutAsync()
        {
            await localStorage.RemoveItemAsync("accessToken");
            await stateProvider.GetAuthenticationStateAsync();
        }

        public async Task GoBackAsync()
        {
            await jSRuntime.InvokeVoidAsync("history.back");
        }

        public void GoTo(string url, bool reload = false, bool replace = false)
        {
            navigationManager.NavigateTo(url, reload, replace);
        }

        public async Task CancelModalAsync(BlazoredModalInstance instance)
        {
            await instance.CancelAsync();
        }

        //public Stream ToStream(IBrowserFile file)
        //{
        //    var stream = file.OpenReadStream();
        //    //var stream = new MemoryStream();
        //    //file.CopyToAsync(stream);
        //    stream.Position = 0;
        //} 
    }
}