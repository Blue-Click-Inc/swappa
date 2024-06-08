using Blazored.Modal;
using Microsoft.AspNetCore.Components;
using Swappa.Shared.DTOs;
using Swappa.Shared.Extensions;

namespace Swappa.Client.Pages.Modals.User
{
    public partial class EditUserDetailsModal
    {
        [CascadingParameter]
        BlazoredModalInstance Instance { get; set; } = new();
        public UserDetailsForUpdateDto? UserDetail { get; set; }
        public ResponseModel<string>? Response { get; set; }
        private bool isLoading = true;
        private string message = string.Empty;
        private string buttonLabel = "Save";
        private bool isButtonDisabled = false;
        private string pageTitle = "Edit User Details";

        protected override async Task OnInitializedAsync()
        {
            var userId = await UserService.GetLoggedInUserId();
            var userDetails = await UserService.GetUserByIdAsync(userId);
            if (userDetails != null && userDetails.IsSuccessful)
            {
                UserDetail = new UserDetailsForUpdateDto
                {
                    Name = userDetails.Data?.Name!,
                    Gender = userDetails.Data.Gender
                };
            }
            else
            {
                message = (userDetails == null || string.IsNullOrEmpty(userDetails.Message)) ? 
                    "An error occured while getting user details for edit. Please try again." : userDetails.Message;
                Toast.ShowError(message);
                await Instance.CancelAsync();
            }

            isLoading = false;
            await base.OnInitializedAsync();
        }

        async Task SaveAsync()
        {
            var userId = await UserService.GetLoggedInUserId();
            if (userId.IsNotEmpty() && UserDetail != null)
            {
                isButtonDisabled = Response == null;

                Response = await UserService.UpdateDetailsAsync(userId, UserDetail);
                if (Response != null && Response.IsSuccessful)
                {
                    message = Response.Data ?? "Operation successful.";
                    Toast.ShowSuccess(message);
                    await Instance.CloseAsync();
                }
                else
                {
                    message = Response?.Message ?? "An error occured while getting user details for edit. Please try again.";
                    Toast.ShowError(message);
                }
            }
            else
            {
                Toast.ShowError("Invalid userId supplied. Please try again.");
            }

            isButtonDisabled = false;
        }
    }
}
