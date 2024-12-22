using Blazored.Modal;
using Microsoft.AspNetCore.Components;
using Swappa.Client.State;
using Swappa.Shared.DTOs;
using Swappa.Shared.Extensions;

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
                    var userId = AccountService
                        .GetUserIdFromToken(Response.Data?.AccessToken ?? string.Empty)
                        .ToGuid();
                    await LocalStorage.SetItemAsync("userId", userId);
                    await LocalStorage.SetItemAsync("accessToken", Response.Data?.AccessToken);
                    await LocalStorage.SetItemAsync("refreshToken", Response.Data?.RefreshToken);

                    await AuthStateProvider.GetAuthenticationStateAsync();
                    if (userId.IsNotEmpty())
                    {
                        GlobalVariables.UserId = userId;
                        var countResponse = await VehicleService.GetFavoriteCount(userId);
                        if (countResponse.IsSuccessful)
                        {
                            GlobalVariables.Favorites = countResponse.Data;
                        }
                    }
                    await Instance.CloseAsync();
                    Toast.ShowSuccess($"Login successful.");
                }
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
