using Swappa.Client.Pages.Modals.Vehicle;
using Swappa.Shared.DTOs;

namespace Swappa.Client.Pages.Vehicle
{
    public partial class VehicleAdmin
    {
        private bool isLoading = false;
        private string message = string.Empty;

        public PaginatedListDto<VehicleToReturnDto>? Data { get; set; }
        
        protected override async Task OnInitializedAsync()
        {
            await GetDataAsync();
            await base.OnInitializedAsync();
        }
        private async Task ShowBulkVehicleUploadModal()
        {
            var confirmation = Modal.Show<BulkVehicleUploadModal>("");
            await confirmation.Result;
        }

        private async Task GetDataAsync()
        {
            isLoading = Data == null;
            var response = await VehicleService.GetDataAsync(new VehicleQueryDto());
            if(response != null && response.IsSuccessful)
            {
                Data = response.Data;
            }
            else
            {
                message = response?.Message ?? string.Empty;
            }
            isLoading = false;
        }
    }
}
