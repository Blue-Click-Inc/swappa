using Microsoft.JSInterop;
using Swappa.Shared.DTOs;
using Swappa.Shared.Extensions;

namespace Swappa.Client.Pages.Account
{
    public partial class Profile
    {
        private string _message = "Something went wrong.";
        private bool isLoading = true;

        public UserDetailsDto? Data { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var userId = await UserService.GetLoggedInUserId();
            await GetUserDetails(userId);
            await base.OnInitializedAsync();
        }

        public async Task GetUserDetails(Guid id)
        {
            if (id.IsEmpty())
            {
                _message = string.Concat(_message, " Invalid UserId");
                Toast.ShowError(_message); 
                await JSRuntime.InvokeVoidAsync("history.back");
                isLoading = false;
                return;
            }

            var result = await UserService.GetUserByIdAsync(id);
            if(result == null || !result.IsSuccessful)
            {
                _message = result == null || string.IsNullOrEmpty(result.Message) ?
                    _message :
                    result.Message;

                Toast.ShowError(_message);
                await JSRuntime.InvokeVoidAsync("history.back");
                isLoading = false;
                return;
            }

            Data = result.Data;
            isLoading = false;
        }
    }
}
