using Swappa.Shared.DTOs;

namespace Swappa.Client.Pages.Admin
{
    public partial class AdminCenter
    {
        private bool _isUserLoading = false;
        private bool _isFeedbackLoading = false;
        private bool _isVehicleLoading = false;

        public UserDashboardDataDto? UserDashboard { get; set; }
        public FeedbackDashboardDto? FeedbackDashboard { get; set; }
        public VehicleDashboardDto? VehicleDashboard { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await GetVehicleDashBoard();
            await GetUsersDashBoard();
            await GetFeedbacksDashBoard();
            await base.OnInitializedAsync();
        }

        private async Task GetVehicleDashBoard()
        {
            _isVehicleLoading = true;
            var result = await VehicleService.GetDashboard();
            if(result != null && result.IsSuccessful)
            {
                VehicleDashboard = result.Data;
            }
            else
            {
                Toast.ShowError(result?.Message ?? "An error occurred. Could not get Vehicle dashboard data");
            }

            _isVehicleLoading = false;
        }

        private async Task GetFeedbacksDashBoard()
        {
            _isFeedbackLoading = true;
            var result = await UserService.FeedbackDashboardData();
            if (result != null && result.IsSuccessful)
            {
                FeedbackDashboard = result.Data;
            }
            else
            {
                Toast.ShowError(result?.Message ?? "An error occurred. Could not get Feedbacks dashboard data");
            }
            _isFeedbackLoading = false;
        }

        private async Task GetUsersDashBoard()
        {
            _isUserLoading = true;
            var result = await UserService.GetUserDashboardAsync();
            if (result != null && result.IsSuccessful)
            {
                UserDashboard = result.Data;
            }
            else
            {
                Toast.ShowError(result?.Message ?? "An error occurred. Could not get Users dashboard data");
            }
            _isUserLoading = false;
        }
    }
}
