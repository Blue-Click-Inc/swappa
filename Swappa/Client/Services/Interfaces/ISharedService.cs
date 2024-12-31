using BlazorBootstrap;
using Blazored.Modal;
using Microsoft.AspNetCore.Components.Forms;
using Swappa.Entities.Enums;
using Swappa.Shared.DTOs;

namespace Swappa.Client.Services.Interfaces
{
    public interface ISharedService
    {
        Task CancelModalAsync(BlazoredModalInstance instance);
        ModalParameters GetDialogParameters(string title, string message, string styleClass = "text-danger", ButtonColor buttonColor = ButtonColor.Danger, AlertColor alertColor = AlertColor.Danger);
        Task GoBackAsync();
        void GoTo(string url, bool reload = false, bool replace = false);
        Task LogoutAsync();
        List<string> GetRandomBackgroundColors(int dataLabelsCount, string[]? backgroundColors);
        MultipartFormDataContent OnInputFilesChange(InputFileChangeEventArgs e, FileTypes fileType, string formFileName, long maxAllowedMaximumSize, out bool isValidInputs);
        string GetQuery(PageDto query);
        string GetSubstring(string str);
    }
}
