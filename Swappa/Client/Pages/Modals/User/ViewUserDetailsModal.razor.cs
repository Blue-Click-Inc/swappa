using BlazorBootstrap;
using Blazored.Modal;
using Microsoft.AspNetCore.Components;
using Swappa.Entities.Enums;
using Swappa.Shared.DTOs;
using Swappa.Shared.Extensions;

namespace Swappa.Client.Pages.Modals.User
{
    public partial class ViewUserDetailsModal
    {
        private bool isError = false;
        private string message = string.Empty;
        private string pageTitle = "User Details";
        private string toggleButtonLabel = string.Empty;
        private bool isBusy = false;
        private bool isLoading = false;
        private string roles = string.Empty;

        [CascadingParameter]
        public BlazoredModalInstance Instance { get; set; } = new();
        [Parameter]
        public Guid Id { get; set; }
        public UserDetailsDto? UserData { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await GetUserByIdAsync(Id);
            await base.OnInitializedAsync();
        }

        private ButtonColor GetColor()
        {
            return UserData?.Status == Status.Active ? ButtonColor.Danger : ButtonColor.Success;
        }

        private IconName GetIconName()
        {
            return UserData?.Status == Status.Active ? IconName.Ban : IconName.Check;
        }

        private async Task GetUserByIdAsync(Guid id)
        {
            isLoading = true;
            var userResult = await UserService.GetUserByIdAsync(id);
            if(userResult != null && userResult.IsSuccessful)
            {
                UserData = userResult.Data;
                pageTitle = UserData?.Name ?? "User Details";
                roles = string.Join(", ", UserData?.UserRoles!);
                toggleButtonLabel = UserData?.Status == Status.Active ? "Deactivate" : "Activate";
            }
            else
            {
                isError = true;
                message = userResult?.Message ?? "An error occurred. Could not get user details";
            }

            isLoading = false;
        }

        private async Task ToggleStatus(Guid? id)
        {
            if (id != null)
            {
                isBusy = true;
                var response = await AccountService.ToggleStatus(id.Value);
                if (response != null && response.IsSuccessful)
                {
                    Toast.ShowSuccess(response?.Data ?? "Operation successful");
                    await GetUserByIdAsync(id.Value);
                }
                else
                {
                    Toast.ShowError(response?.Message ?? "An error occurred. Please try again");
                }

                isBusy = false;
            }
            else
            {
                Toast.ShowError($"Invalid parameter: Id");
            }
        }
    }
}
