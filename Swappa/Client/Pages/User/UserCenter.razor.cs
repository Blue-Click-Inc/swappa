using Blazored.Modal;
using Swappa.Client.Pages.Modals.Role;
using Swappa.Client.Pages.Modals.User;
using Swappa.Entities.Enums;
using Swappa.Shared.DTOs;

namespace Swappa.Client.Pages.User
{
    public partial class UserCenter
    {
        private string pageTitle = "Users Management Center";
        private bool isLoading = false;
        private bool isError = false;

        public long TotalUsers { get; set; }
        public long ActiveUsers { get; set; }
        public long InactiveUsers { get; set; }
        public PaginatedListDto<LeanUserDetailsDto>? UserData { get; set; }
        public SearchDto Search { get; set; } = new();

        protected override async Task OnInitializedAsync()
        {
            isLoading = true;
            await GetDashboard();
            await GetPagedUserList();
            isLoading = false;
            await base.OnInitializedAsync();
        }

        private async Task GetDashboard()
        {
            var response = await UserService.GetUserDashboardAsync();
            if(response != null && response.IsSuccessful)
            {
                var data = response.Data;
                TotalUsers = data.TotalCount;
                ActiveUsers = data.ActiveCount;
                InactiveUsers = data.InactiveCount;
            }
            else
            {
                isError = true;
                Toast.ShowError(response?.Message ?? "An error occurred. Please try again later");
            }
        }

        private async Task GetPagedUserList()
        {
            var response = await UserService.GetPagedUsersAsync(Search);
            if (response != null && response.IsSuccessful)
            {
                UserData = response.Data;
            }
            else
            {
                isError = true;
                Toast.ShowError(response?.Message ?? "An error occurred. Please try again later");
            }
        }

        private async Task OpenUserDetailModal(Guid id)
        {
            var parameters = new ModalParameters
            {
                { "Id", id }
            };

            var confirm = Modal.Show<ViewUserDetailsModal>("", parameters);
            var result = await confirm.Result;
            if (result.Confirmed || result.Cancelled)
            {
                isLoading = true;
                await GetDashboard();
                await GetPagedUserList();
                isLoading = false;
            }
        }

        private async Task OpenRoleManagerModal(Guid id)
        {
            var parameters = new ModalParameters
            {
                { "Id", id }
            };

            var confirm = Modal.Show<RoleManagerModal>("", parameters);
            var result = await confirm.Result;
        }

        private string GetStatusClass(Status status)
        {
            switch (status)
            {
                case Status.Inactive:
                    return "danger";
                case Status.Active:
                    return "success";
                default:
                    return "secondary";
            }
        }
    }
}
