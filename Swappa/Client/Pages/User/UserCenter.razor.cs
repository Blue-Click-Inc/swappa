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
    }
}
