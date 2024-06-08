using Blazored.Modal;
using Microsoft.AspNetCore.Components;
using Swappa.Client.Pages.Modals.Accounts;
using Swappa.Entities.Enums;
using Swappa.Shared.DTOs;

namespace Swappa.Client.Pages.Components.Account
{
    public partial class Register
    {
        [CascadingParameter] 
        BlazoredModalInstance Instance { get; set; } = new();
        [Parameter]
        public RegistrationType Type { get; set; }
        public RegisterDto RegisterModel { get; set; } = new();
        public ResponseModel<string>? Response { get; set; }
        private bool isLoading = false;
        private string message = string.Empty;
        private string buttonLabel = "Register";
        private string pageTitle => Type == RegistrationType.Regular ?
            "Register User" :
            "Register Privileged User";

        private async Task RegisterAsync()
        {
            if (Response == null)
                isLoading = true;

            Response = await AccountService.RegisterAsync(RegisterModel);
            if (Response != null)
            {
                if (!Response.IsSuccessful)
                {
                    message = Response?.Message ?? "An error occurred";
                    Toast.ShowError(message);
                }
                else
                {
                    await Instance.CloseAsync();
                    message = Response.Data ?? "Success";
                    Toast.ShowSuccess(message);
                }
            }
            else
            {
                Toast.ShowError("Something went wrong. Please try again");
            }
            isLoading = false;
        }

        private async Task GoToLogin()
        {
            await Instance.CancelAsync();
            Modal.Show<LoginModal>("");
        }
    }
}
