using Microsoft.JSInterop;
using Swappa.Client.Pages.Modals.Vehicle;
using Swappa.Shared.DTOs;
using Swappa.Shared.Extensions;

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

        private async Task ExportToExcel()
        {
            var response = await VehicleService.ExportToExcel();
            if(response.IsNull() || !response.IsSuccessStatusCode)
            {
                Toast.ShowError("An error occurred while exporting the data. Please try again later.");
            }
            else
            {
                var fileStream = await response.Content.ReadAsStreamAsync();
                using var streamRef = new DotNetStreamReference(stream: fileStream);
                await JSRuntime.InvokeVoidAsync("downloadFileFromStream", $"Vehicle Data-{DateTime.UtcNow.Ticks}.xlsx", streamRef);
            }
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
