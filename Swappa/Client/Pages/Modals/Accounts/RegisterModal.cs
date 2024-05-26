using Blazored.Modal;
using Microsoft.AspNetCore.Components;
using Swappa.Shared.DTOs;

namespace Swappa.Client.Pages.Modals.Accounts
{
    public partial class RegisterModal
    {
        [CascadingParameter] BlazoredModalInstance Instance { get; set; }
        public RegisterDto RegisterModel { get; set; } = new();
        private bool isError = false;
        private bool isLoading = false;
        private string message = string.Empty;
        private string buttonLabel = "Register";

        private async Task RegisterAsync()
        {
            // Login
            var result = false;
            if (result)
            {
                await Instance.CloseAsync();
                Toast.ShowSuccess("Registration successful.");
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