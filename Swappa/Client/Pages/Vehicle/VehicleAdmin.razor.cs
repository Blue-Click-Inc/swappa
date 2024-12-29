using Blazored.Modal;
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
        public Guid LoggedInUserId { get; set; }
        public VehicleQueryDto Query { get; set; } = new();
        public string NumberOfVehicles => Data.IsNull() ? 
            0.0m.ToString() : 
            Data.MetaData.TotalCount.ToString("#,##");

        protected override async Task OnInitializedAsync()
        {
            await GetDataAsync();
            LoggedInUserId = await UserService.GetLoggedInUserId();
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

        private async Task PrintPDF()
        {
            var response = await VehicleService.PrintPDF();
            if (response.IsNull() || !response.IsSuccessStatusCode)
            {
                Toast.ShowError("An error occurred while exporting the data. Please try again later.");
            }
            else
            {
                var fileStream = await response.Content.ReadAsStreamAsync();
                using var streamRef = new DotNetStreamReference(stream: fileStream);
                await JSRuntime.InvokeVoidAsync("downloadFileFromStream", $"Vehicle_PDF_Report-{DateTime.UtcNow.Ticks}.pdf", streamRef);
            }
        }

        private async Task ShowBulkVehicleUploadModal()
        {
            var confirmation = Modal.Show<BulkVehicleUploadModal>("");
            await confirmation.Result;
        }

        private async Task ShowAddVehicleModal()
        {
            var idParam = new ModalParameters
            {
                { "UserId", LoggedInUserId }
            };

            var confirmationModal = Modal.Show<AddVehicleModal>("", idParam);
            var result = await confirmationModal.Result;
            if (result.Confirmed)
            {
                await GetDataAsync();
            }
        }

        private async Task GetDataAsync()
        {
            isLoading = Data == null;
            var response = await VehicleService.GetDataAsync(Query);
            if(response != null && response.IsSuccessful)
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

        private async Task OnPageChangedAsync(int newPageNumber)
        {
            Query.PageNumber = newPageNumber;
            await GetDataAsync();
        }

        private async Task Search()
        {
            await GetDataAsync();
        }

        private async Task Clear()
        {
            Query = new();
            await GetDataAsync();
        }
    }
}
