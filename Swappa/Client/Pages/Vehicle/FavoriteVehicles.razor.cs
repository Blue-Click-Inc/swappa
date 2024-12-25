using Swappa.Shared.DTOs;
using Swappa.Shared.Extensions;

namespace Swappa.Client.Pages.Vehicle
{
    public partial class FavoriteVehicles
    {
        private bool isLoading = false;
        private string message = string.Empty;

        public PaginatedListDto<VehicleToReturnDto>? Data { get; set; }
        public Guid LoggedInUserId { get; set; }
        public VehicleQueryDto Query { get; set; } = new();
        public string NumberOfVehicles => Data.IsNull() ?
            0.0m.ToString() :
            Data.MetaData.TotalCount.ToString("#,##");

        protected override async Task OnInitializedAsync()
        {
            LoggedInUserId = await UserService.GetLoggedInUserId();
            await GetDataAsync(LoggedInUserId);
            await base.OnInitializedAsync();
        }

        private async Task GetDataAsync(Guid userId)
        {
            isLoading = Data == null;
            var response = await VehicleService.GetFavoriteDataAsync(userId, Query);
            if (response != null && response.IsSuccessful)
            {
                Data = response.Data;
            }
            else
            {
                message = response?.Message ?? string.Empty;
                Toast.ShowError(message);
            }
            isLoading = false;
        }

        private async Task Search()
        {
            await GetDataAsync(LoggedInUserId);
        }

        private async Task Clear()
        {
            Query = new();
            await GetDataAsync(LoggedInUserId);
        }
    }
}
