using Blazored.Modal;
using Microsoft.AspNetCore.Components;
using Swappa.Shared.DTOs;

namespace Swappa.Client.Pages.Modals.User
{
    public partial class UserFeedbackModal
    {
        private bool isLoading = false;
        private string message = string.Empty;
        private string buttonLabel = "Send";
        private string pageTitle = "Leave A Feedback";

        [CascadingParameter]
        BlazoredModalInstance Instance { get; set; } = new();
        [Parameter]
        public string Email { get; set; } = string.Empty;
        public FeedbackForAddDto Feedback { get; set; } = new();
        public ResponseModel<string>? Response { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Feedback.UserEmail = Email;
            await base.OnInitializedAsync();
        }

        async Task SendAsync()
        {
            if(Response == null)
            {
                isLoading = true;
            }

            Response = await UserService.SendFeedbackAsync(Feedback);
            if (Response != null && Response.IsSuccessful)
            {
                message = Response.Data ?? "Operation successful.";
                Toast.ShowSuccess(message);
                await Instance.CloseAsync();
            }
            else
            {
                message = Response?.Message ?? "An error occured while sending your feedback. Please try again.";
                Toast.ShowError(message);
            }

            isLoading = false;
        }
    }
}
