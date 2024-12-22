using BlazorBootstrap;
using Blazored.Modal;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Swappa.Entities.Enums;
using Swappa.Shared.DTOs;
using Swappa.Shared.Extensions;

namespace Swappa.Client.Pages.Modals.Vehicle
{
    public partial class BulkVehicleUploadModal
    {
        private bool isLoading = false;
        private string message = string.Empty;
        private AlertColor level = AlertColor.None;
        private const int MAX_SIZE = 5242880;

        [CascadingParameter]
        public BlazoredModalInstance Instance { get; set; } = new();
        public FileDto Model { get; set; } = new();
        public MultipartFormDataContent? Content { get; set; }
        public ResponseModel<string>? Response { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
        }

        private async Task UploadAsync()
        {
            isLoading = true;
            if (Content.IsNotNull())
            {
                Response = await ToolService.UploadBulkVehicle(Content);
                if(Response != null && Response.IsSuccessful)
                {
                    Toast.ShowSuccess(Response.Data ?? "File upload successful");
                    await Instance.CloseAsync();
                }
                else
                {
                    Toast.ShowError(Response?.Message ?? string.Empty);
                }
            }
            else
            {
                message = "Please choose a valid file to continue";
                level = AlertColor.Danger;
            }

            isLoading = false;
        }

        private void OnInputFileChange(InputFileChangeEventArgs e)
        {
            Content = SharedService.OnInputFilesChange(e, FileTypes.Sheet, "files", MAX_SIZE,  out var isValidInput);
            if (!isValidInput)
            {
                message = $"File size must not exceed {FileTypes.Sheet.MaxSize()}MiB and must be one of ({FileTypes.Sheet.AllowedFileType()})";
                level = AlertColor.Warning;
            }
            this.StateHasChanged();
        }
    }
}
