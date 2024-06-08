using Blazored.Modal;
using Microsoft.AspNetCore.Components;
using Swappa.Shared.DTOs;

namespace Swappa.Client.Pages.Modals.Accounts
{
    public partial class LoginModal
    {
        [CascadingParameter] BlazoredModalInstance Instance { get; set; } = new();
        public LoginDto LoginModel { get; set; } = new();
        public ResponseModel<TokenDto>? Response { get; set; }
        private bool isLoading = false;
        private string buttonLabel = "Login";

        private async Task LoginAsync()
        {
            if(Response == null)
                isLoading = true;

            Response = await AccountService.LoginAsync(LoginModel);
            if(Response != null)
            {
                if (!Response.IsSuccessful)
                {
                    Toast.ShowError(Response?.Message!);
                }
                else
                {
                    await LocalStorage.SetItemAsync("accessToken", Response.Data?.AccessToken);
                    await LocalStorage.SetItemAsync("refreshToken", Response.Data?.RefreshToken);
                    await AuthStateProvider.GetAuthenticationStateAsync();
                    await Instance.CloseAsync();
                    Toast.ShowSuccess("Login successful.");
                }
            }
            else
            {
                Toast.ShowError("Something went wrong. Please try again");
            }
            isLoading = false;
        }

        private async Task GoToRegister()
        {
            await SharedService.CancelModalAsync(Instance);
            Modal.Show<RegisterModal>("");
        }

        private async Task GoToResetPassword()
        {
            await SharedService.CancelModalAsync(Instance);
            Modal.Show<ResetPasswordModal>("");
        }
    }
}
