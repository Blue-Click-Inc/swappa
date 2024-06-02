using Blazored.Modal;
using Microsoft.AspNetCore.Components;
using Swappa.Shared.DTOs;

namespace Swappa.Client.Pages.Modals.Accounts
{
    public partial class ResetPasswordModal
    {
        private bool isLoading = false;
        private string message = string.Empty;
        private string buttonLabel = "Reset";
        private string pageTitle = "Reset Password";
        [CascadingParameter] BlazoredModalInstance Instance { get; set; } = new();
        public EmailDto ResetPasswordModel { get; set; } = new();
        public ResponseModel<string>? Response { get; set; }

        private async Task ResetPasswordAsync()
        {
            if(Response == null)
            {
                isLoading = true;
            }

            Response = await AccountService.ResetPasswordAsync(ResetPasswordModel);
            if (Response != null && Response.IsSuccessful)
            {
                message = Response.Data ?? "Operation successful";
                await Instance.CloseAsync();
                Toast.ShowSuccess(message);
            }
            else
            {
                message = Response?.Message ?? "Sorry, an error occurred";
                Toast.ShowError(message);
            }

            isLoading = false;
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
