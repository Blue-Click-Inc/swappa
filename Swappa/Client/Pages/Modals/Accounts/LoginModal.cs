using Blazored.Modal;
using Microsoft.AspNetCore.Components;
using Swappa.Shared.DTOs;

namespace Swappa.Client.Pages.Modals.Accounts
{
    public partial class LoginModal
    {
        [CascadingParameter] BlazoredModalInstance Instance { get; set; }
        public LoginDto LoginModel { get; set; } = new();
        private bool isError = false;
        private bool isLoading = false;
        private string message = string.Empty;
        private string buttonLabel = "Login";

        private async Task LoginAsync()
        {
            // Login
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

        private async Task GoToRegister()
        {
            await Instance.CancelAsync();
            Modal.Show<RegisterModal>("");
        }

        private async Task GoToForgotPassword()
        {
            await Instance.CancelAsync();
            Modal.Show<ForgotPasswordModal>("");
        }
    }
}
