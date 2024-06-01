using Blazored.Modal;
using Microsoft.AspNetCore.Components;
using Swappa.Shared.DTOs;

namespace Swappa.Client.Pages.Modals.Accounts
{
    public partial class ResetPasswordModal
    {
        [CascadingParameter] BlazoredModalInstance Instance { get; set; } = new();
        public EmailDto ResetPasswordModel { get; set; } = new();
        private bool isError = false;
        private bool isLoading = false;
        private string message = string.Empty;
        private string buttonLabel = "Reset";

        private async Task ResetPasswordAsync()
        {
            var result = true;
            if (result)
            {
                await Instance.CloseAsync();
                Toast.ShowSuccess("Login successful.");
            }
            else
            {
                Toast.ShowError("Wrong username or password.");
            }
        }

        private async Task GoBack()
        {
            await Instance.CancelAsync();
        }

        private async Task GoToLogin()
        {
            await Instance.CancelAsync();
            Modal.Show<LoginModal>("");
        }
    }
}
