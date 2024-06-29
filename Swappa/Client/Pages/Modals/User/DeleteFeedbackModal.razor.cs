using Blazored.Modal;
using Microsoft.AspNetCore.Components;
using Swappa.Shared.DTOs;

namespace Swappa.Client.Pages.Modals.User
{
    public partial class DeleteFeedbackModal
    {
        private bool isLoading = false;
        private string message = string.Empty;
        private string buttonLabel = "Delete";
        private string pageTitle = "Delete Feedback";

        [CascadingParameter]
        BlazoredModalInstance Instance { get; set; } = new();
        [Parameter]
        public Guid Id { get; set; }
        public ResponseModel<string>? Response { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
        }

        async Task DoAsync()
        {
            if (Response == null)
            {
                isLoading = true;
            }

            Response = await UserService.DeleteFeedbackAsync(Id);
            if (Response != null && Response.IsSuccessful)
            {
                message = Response.Data ?? "Operation successful.";
                Toast.ShowSuccess(message);
                await Instance.CloseAsync();
            }
            else
            {
                message = Response?.Message ?? "An error occured while deleting your feedback. Please try again.";
                Toast.ShowError(message);
            }

            isLoading = false;
        }
    }
}
