using Blazored.Modal;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Swappa.Entities.Enums;
using Swappa.Shared.DTOs;

namespace Swappa.Client.Pages.Modals.Vehicle
{
    public partial class AddVehicleModal
    {
        private const long MAX_FILE_SIZE = 2097152;
        private readonly long _maxFileSizeInMb = MAX_FILE_SIZE / 1024 / 1024;
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
        public VehicleToCreateDto Request { get; set; } = new();
        public ResponseModel<string>? Response { get; set; }
        public MultipartFormDataContent FormData { get; set; } = new();
        private bool isLoading = false;
        private string message = string.Empty;
        private string buttonLabel = "Add";
        private string pageTitle => "Add A Vehicle";

        private async Task Save()
        {
            OnClickToAdd();
            isLoading = true;

            Response = await VehicleService.AddAsync(FormData);
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

        private void OnClickToAdd()
        {
            FormData.Add(new StringContent(Request.Make), nameof(VehicleToCreateDto.Make));
            FormData.Add(new StringContent(Request.Color), nameof(VehicleToCreateDto.Color));
            FormData.Add(new StringContent(Request.VIN ?? string.Empty), nameof(VehicleToCreateDto.VIN));
            FormData.Add(new StringContent(Request.Interior ?? string.Empty), nameof(VehicleToCreateDto.Interior));
            FormData.Add(new StringContent(Request.DriveTrain.ToString()), nameof(VehicleToCreateDto.DriveTrain));
            FormData.Add(new StringContent(Request.Engine.ToString()), nameof(VehicleToCreateDto.Engine));
            FormData.Add(new StringContent(Request.Transmission.ToString()), nameof(VehicleToCreateDto.Transmission));
            FormData.Add(new StringContent(Request.Model), nameof(VehicleToCreateDto.Model));
            FormData.Add(new StringContent(Request.Odometer.ToString()), nameof(VehicleToCreateDto.Odometer));
            FormData.Add(new StringContent(Request.Price.ToString()), nameof(VehicleToCreateDto.Price));
            FormData.Add(new StringContent(Request.Year.ToString()), nameof(VehicleToCreateDto.Year));
            FormData.Add(new StringContent(Request.Trim.ToString()), nameof(VehicleToCreateDto.Trim));
        }

        private void OnInputFileChange(InputFileChangeEventArgs e)
        {
            FormData = SharedService.OnInputFilesChange(e, FileTypes.Image, "Images", MAX_FILE_SIZE, out var isValidInput);
            if (!isValidInput)
            {
                message = $"File size must not exceed {_maxFileSizeInMb}MiB.";
                Toast.ShowWarning(message);
            }
            this.StateHasChanged();
        }
    }
}
