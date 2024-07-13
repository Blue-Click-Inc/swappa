using Blazored.Modal;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;

namespace Swappa.Client.Services.Interfaces
{
    public interface ISharedService
    {
        Task CancelModalAsync(BlazoredModalInstance instance);
        Task GoBackAsync();
        void GoTo(string url, bool reload = false, bool replace = false);
        Task LogoutAsync();
    }
}
