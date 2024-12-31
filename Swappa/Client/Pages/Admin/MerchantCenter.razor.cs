using Blazored.Modal;
using Swappa.Client.Extensions;
using Swappa.Client.Pages.Modals.Vehicle;
using Swappa.Shared.DTOs;
using Swappa.Shared.Extensions;

namespace Swappa.Client.Pages.Admin
{
    public partial class MerchantCenter
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
            await GetDataAsync();
            await base.OnInitializedAsync();
        }

        private async Task ShowAddVehicleModal()
        {
            var confirmed = await Modal.ShowAddVehicleModal(LoggedInUserId);
            if (confirmed)
            {
                await GetDataAsync();
            }
        }

        private async Task GetDataAsync()
        {
            isLoading = Data == null;
            LoggedInUserId = await UserService.GetLoggedInUserId();
            var response = await VehicleService.GetMerchantVehicleDataAsync(LoggedInUserId, Query);
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
    }
}
