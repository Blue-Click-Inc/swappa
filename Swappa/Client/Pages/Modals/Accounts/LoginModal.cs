﻿using Blazored.Modal;
using Microsoft.AspNetCore.Components;
using Swappa.Client.State;
using Swappa.Shared.DTOs;

namespace Swappa.Client.Pages.Modals.Accounts
{
    public partial class LoginModal
    {
        [CascadingParameter] BlazoredModalInstance Instance { get; set; }
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
                    var authProvider = (CustomAuthenticationStateProvider)AuthStateProvider;
                    authProvider.UpdateAuthenticationState(Response.Data?.AccessToken!);
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
