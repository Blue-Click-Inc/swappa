using Blazored.Modal;
using Microsoft.AspNetCore.Components;
using Swappa.Shared.DTOs;

namespace Swappa.Client.Pages.Modals.Accounts
{
    public partial class ForgotPasswordModal
    {
        private bool isLoading = false;
        private string buttonLabel = "Save";
        private string pageTitle = "Change Forgotten Password";

        [CascadingParameter] BlazoredModalInstance Instance { get; set; } = new();
        public ForgotPasswordDto RequestModel { get; set; } = new();
        public ResponseModel<string>? Response { get; set; }
        [Parameter] 
        public string Token { get; set; } = string.Empty;

        protected override async Task OnInitializedAsync()
        {
            RequestModel.Token = Token;
            await base.OnInitializedAsync();
        }

        private async Task ChangeForgotPasswordAsync()
        {
            if(Response == null)
            {
                isLoading = true;
            }

            Response = await AccountService.ChangeForgotPasswordAsync(RequestModel);
            if (Response != null)
            {
                if (!Response.IsSuccessful)
                {
                    Toast.ShowError(Response?.Message!);
                }
                else
                {
                    await Instance.CloseAsync();
                    Toast.ShowSuccess(Response?.Data!);
                }
            }
            else
            {
                Toast.ShowError("Something went wrong. Please try again");
            }
            isLoading = false;
            NavManager.NavigateTo("/", replace: true);
        }

        private async Task GoToLogin()
        {
            await Instance.CloseAsync();
            Modal.Show<LoginModal>("");
        }

        private async Task Cancel()
        {
            await Instance.CloseAsync();
            NavManager.NavigateTo("/", replace: true);
        }
    }
}
