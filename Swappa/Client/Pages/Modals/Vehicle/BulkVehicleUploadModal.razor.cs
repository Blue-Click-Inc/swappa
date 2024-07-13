using BlazorBootstrap;
using Blazored.Modal;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Swappa.Entities.Enums;
using Swappa.Shared.DTOs;
using Swappa.Shared.Extensions;
using System.Net.Http.Headers;

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
            var content = new MultipartFormDataContent();
            var file = e.GetMultipleFiles().FirstOrDefault();
            if (file.IsValid(FileTypes.Sheet))
            {
                var stream = new StreamContent(file.OpenReadStream(MAX_SIZE));
                stream.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
                
                content.Add(
                    content: stream,
                    name: "\"files\"",
                    fileName: file.Name);

                Content = content;
            }
            else
            {
                message = $"File size must not exceed {FileTypes.Sheet.MaxSize()}MiB and must be one of ({FileTypes.Sheet.AllowedFileType()})";
                level = AlertColor.Warning;
            }
            this.StateHasChanged();
        }
    }
}
