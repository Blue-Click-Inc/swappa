using Blazored.Modal;
using Microsoft.AspNetCore.Components;
using Swappa.Entities.Enums;
using Swappa.Shared.DTOs;

namespace Swappa.Client.Pages.Modals.User
{
    public partial class DeprecateFeedbackModal
    {
        private bool isLoading = false;
        private string message = string.Empty;
        private string iconClass = string.Empty;

        [CascadingParameter]
        BlazoredModalInstance Instance { get; set; } = new();
        [Parameter]
        public Guid Id { get; set; }
        [Parameter]
        public string Action { get; set; } = string.Empty;
        public string PageTitle 
        {
            get { return $"{Action} Feedback"; }
        }
        public ResponseModel<string>? Response { get; set; }

        protected override async Task OnInitializedAsync()
        {
            iconClass = Action.ToLower() == "deprecate" ? "oi-action-undo" : "oi-action-redo";
            await base.OnInitializedAsync();
        }

        async Task DoAsync()
        {
            if (Response == null)
            {
                isLoading = true;
            }

            Response = await UserService.ToggleFeedbackAsync(Id);
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