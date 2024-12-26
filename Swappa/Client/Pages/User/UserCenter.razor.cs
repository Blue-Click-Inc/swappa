using Blazored.Modal;
using Swappa.Client.Pages.Modals.Accounts;
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
        public PageDto Query { get; set; } = new();

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
            var response = await UserService.GetPagedUsersAsync(Query);
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

        private async Task Search()
        {
            await GetPagedUserList();
        }

        private async Task Clear()
        {
            Query = new();
            await GetPagedUserList();
        }

        private async Task OnPageChangedAsync(int newPageNumber)
        {
            Query.PageNumber = newPageNumber;
            await GetPagedUserList();
        }

        private async Task ShowAddUserModal()
        {
            var parameters = new ModalParameters
            {
                { "ForSuperUser", true }
            };
            var confirmation = Modal.Show<RegisterDialogue>("", parameters);
            await confirmation.Result;
        }
    }
}
