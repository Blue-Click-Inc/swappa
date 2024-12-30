using BlazorBootstrap;
using Blazored.LocalStorage;
using Blazored.Modal;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using Swappa.Client.Services.Interfaces;
using Swappa.Client.State;
using Swappa.Entities.Enums;
using Swappa.Shared.DTOs;
using Swappa.Shared.Extensions;
using System.Net.Http.Headers;
using System.Text;

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
            GlobalVariables.UserId = Guid.Empty;
            GlobalVariables.Favorites = 0;
            await localStorage.RemoveItemAsync("accessToken");
            await localStorage.RemoveItemAsync("refreshToken");
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

        public ModalParameters GetDialogParameters(string title, string message, 
            string styleClass = "text-danger", ButtonColor buttonColor = ButtonColor.Danger,
            AlertColor alertColor = AlertColor.Danger)
        {
            return new ModalParameters
            {
                { "Title", title },
                { "Message", message },
                { "Class", styleClass },
                { "ButtonColor", buttonColor },
                { "AlertColor", alertColor}
            };
        }

        public List<string> GetRandomBackgroundColors(int dataLabelsCount, string[]? backgroundColors)
        {
            var colors = new List<string>();
            for (var index = 0; index < dataLabelsCount; index++)
            {
                colors.Add(backgroundColors![index]);
            }

            return colors;
        }

        public async Task CancelModalAsync(BlazoredModalInstance instance)
        {
            await instance.CancelAsync();
        }

        public string GetQuery(PageDto query)
        {
            var queryStr = string.Empty;
            if (query.SearchTerm.IsNotNullOrEmpty())
            {
                queryStr += queryStr.IsNotNullOrEmpty() ? $"&SearchTerm={query.SearchTerm}" : $"?SearchTerm={query.SearchTerm}";
            }
            if (query.PageSize != default)
            {
                queryStr += queryStr.IsNotNullOrEmpty() ? $"&PageSize={query.PageSize}" : $"?PageSize={query.PageSize}";
            }
            if (query.PageNumber != default)
            {
                queryStr += queryStr.IsNotNullOrEmpty() ? $"&PageNumber={query.PageNumber}" : $"?PageNumber={query.PageNumber}";
            }

            return queryStr;
        }

        public string GetSubstring(string str)
        {
            var allowedLength = 30;
            var result = new StringBuilder();
            if (str.Length > allowedLength)
            {
                result = result.AppendFormat("{0}...", str.Substring(0, allowedLength));
            }
            else
            {
                result = result.Append(str);
            }
            return result.ToString();
        }

        public MultipartFormDataContent OnInputFilesChange(InputFileChangeEventArgs e, FileTypes fileType, string formFileName,  long maxAllowedMaximumSize, out bool isValidInputs)
        {
            var content = new MultipartFormDataContent();
            var files = e.GetMultipleFiles().ToList();
            if (files.IsNotNullOrEmpty())
            {
                foreach (var file in files)
                {
                    if (file.IsValid(fileType))
                    {
                        var stream = new StreamContent(file.OpenReadStream(maxAllowedMaximumSize));
                        stream.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);

                        content.Add(
                            content: stream,
                            name: $"\"{formFileName}\"",
                            fileName: file.Name);

                    }
                    else
                    {
                        isValidInputs = false;
                        return content;
                    }
                }

                isValidInputs = true;
                return content;
            }
            else
            {
                isValidInputs = false;
                return content;
            }
        }
    }
}