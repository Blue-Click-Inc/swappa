using Blazored.Modal;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Swappa.Entities.Enums;

namespace Swappa.Client.Services.Interfaces
{
    public interface ISharedService
    {
        Task CancelModalAsync(BlazoredModalInstance instance);
        Task GoBackAsync();
        void GoTo(string url, bool reload = false, bool replace = false);
        Task LogoutAsync();
        MultipartFormDataContent OnInputFilesChange(InputFileChangeEventArgs e, FileTypes fileType, string formFileName, long maxAllowedMaximumSize, out bool isValidInputs);
    }
}
