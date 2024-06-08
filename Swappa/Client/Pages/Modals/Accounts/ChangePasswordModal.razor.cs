using Blazored.Modal;
using Microsoft.AspNetCore.Components;
using Swappa.Shared.DTOs;
using Swappa.Shared.Extensions;

namespace Swappa.Client.Pages.Modals.Accounts
{
    public partial class ChangePasswordModal
    {
        private bool isLoading = false;
        private string message = string.Empty;
        private string buttonLabel = "Change";
        private string pageTitle = "Change Your Password";

        [CascadingParameter]
        BlazoredModalInstance Instance { get; set; } = new();
        [Parameter]
        public Guid Id { get; set; }
        public ResponseModel<string>? Response { get; set; }
        public ChangePasswordDto Request { get; set; } = new();

        async Task ChangePasswordAsync()
        {
            isLoading = true;
            if (Id.IsNotEmpty())
            {
                var response = await AccountService.ChangePasswordAsync(Id, Request);
                if (response != null && response.IsSuccessful)
                {
                    message = response.Data ?? "Operation successful.";
                    Toast.ShowSuccess(message);
                    await Instance.CloseAsync();
                }
                else
                {
                    message = response?.Message ?? "An error occured while processing your request. Please try again.";
                    Toast.ShowError(message);
                }
            }
            else
            {
                Toast.ShowError("Invalid userId provided. Please try again.");
            }

            isLoading = false;
        }
    }
}
