using Blazored.Modal;
using Microsoft.JSInterop;
using Swappa.Client.Pages.Modals.Accounts;
using Swappa.Client.Pages.Modals.Location;
using Swappa.Client.Pages.Modals.User;
using Swappa.Client.Pages.Modals.Vehicle;
using Swappa.Entities.Enums;
using Swappa.Shared.DTOs;
using Swappa.Shared.Extensions;

namespace Swappa.Client.Pages.Account
{
    public partial class Profile
    {
        private string _message = "Something went wrong.";
        private bool isLoading = true;
        private ModalParameters parameters = null!;

        public UserDetailsDto? Data { get; set; }
        public string PageTitle => Data?.Name ?? "Profile Page";

        protected override async Task OnInitializedAsync()
        {
            var userId = await UserService.GetLoggedInUserId();
            await GetUserDetails(userId);
            parameters = new ModalParameters
            {
                { "Id", userId }
            };

            await base.OnInitializedAsync();
        }

        public async Task GetUserDetails(Guid id)
        {
            if (id.IsEmpty())
            {
                _message = string.Concat(_message, " Invalid UserId");
                Toast.ShowError(_message); 
                await JSRuntime.InvokeVoidAsync("history.back");
                isLoading = false;
                return;
            }

            var result = await UserService.GetUserByIdAsync(id);
            if(result == null || !result.IsSuccessful)
            {
                _message = result == null || string.IsNullOrEmpty(result.Message) ?
                    _message :
                    result.Message;

                Toast.ShowError(_message);
                await JSRuntime.InvokeVoidAsync("history.back");
                isLoading = false;
                return;
            }

            Data = result.Data;
            isLoading = false;
        }

        public async Task GoToUserDetailsEdit(Guid id)
        {
            var confirmationModal = Modal.Show<EditUserDetailsModal>("");
            var result = await confirmationModal.Result;
            if (result.Confirmed)
            {
                await GetUserDetails(id);
            }
        }

        public async Task ChangePassword(Guid id)
        {
            var confirmationModal = Modal.Show<ChangePasswordModal>("", parameters);
            var result = await confirmationModal.Result;
            if (result.Confirmed)
            {
                await GetUserDetails(id);
            }
        }

        public async Task UpdateLocation(Guid id)
        {
            var parameters = new ModalParameters
            {
                { "EntityId", Data.Id },
                { "EntityType", EntityType.User }
            };

            var confirmationModal = Modal.Show<UpdateLocationModal>("", parameters);
            var result = await confirmationModal.Result;
            if (result.Confirmed)
            {
                await GetUserDetails(id);
            }
        }

        public async Task AddLocation(Guid id)
        {
            var parameters = new ModalParameters
            {
                { "EntityId", Data.Id },
                { "EntityType", EntityType.User }
            };

            var confirmationModal = Modal.Show<AddLocationModal>("", parameters);
            var result = await confirmationModal.Result;
            if (result.Confirmed)
            {
                await GetUserDetails(id);
            }
        }

        public async Task DeactivateAccount(Guid id)
        {
            var confirmationModal = Modal.Show<DeactivateAccountModal>("", parameters);
            var result = await confirmationModal.Result;
            if (result.Confirmed)
            {
                await SharedService.LogoutAsync();
                SharedService.GoTo("/", true, true);
            }
        }

        public async Task LeaveFeedback(Guid id)
        {
            var emailParam = new ModalParameters
            {
                { "Email", Data.Email }
            };

            var confirmationModal = Modal.Show<UserFeedbackModal>("", emailParam);
            var result = await confirmationModal.Result;
            if (result.Confirmed)
            {
                await GetUserDetails(id);
            }
        }
    }
}
