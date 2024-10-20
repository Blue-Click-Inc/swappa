using Blazored.Modal;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Swappa.Shared.DTOs;

namespace Swappa.Client.Pages.Modals.Vehicle
{
    public partial class AddVehicleModal
    {
        private const long MAX_FILE_SIZE = 2097152;
        private const int MAX_ALLOWED_FILES = 5;
        private const int ONE_GIG = 1024 * 1024;
        private List<string> SUPPORTED_FILES = new List<string>
        {
            ".jpg",
            ".jpeg",
            ".png"
        };

        [CascadingParameter]
        BlazoredModalInstance Instance { get; set; } = new();
        [Parameter]
        public Guid UserId { get; set; }
        public VehicleToCreateDto VehicleForCreateDto { get; set; } = new();
        public ResponseModel<string>? Response { get; set; }
        private bool isLoading = false;
        private string message = string.Empty;
        private string buttonLabel = "Add";
        private string pageTitle => "Add A Vehicle";

        private async Task Save()
        {
            isLoading = true;

            Response = await VehicleService.AddAsync(VehicleForCreateDto);
            if (Response != null)
            {
                if (!Response.IsSuccessful)
                {
                    message = Response?.Message ?? "An error occurred";
                    Toast.ShowError(message);
                }
                else
                {
                    await Instance.CloseAsync();
                    message = Response.Data ?? "Success";
                    Toast.ShowSuccess(message);
                }
            }
            else
            {
                Toast.ShowError("Something went wrong. Please try again");
            }
            isLoading = false;
        }

        private void OnInputFileChange(InputFileChangeEventArgs e)
        {
            
        }

    }
}
