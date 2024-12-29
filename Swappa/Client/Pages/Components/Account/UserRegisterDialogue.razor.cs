using Blazored.Modal;
using Swappa.Client.Pages.Modals.Accounts;
using System.Drawing.Drawing2D;

namespace Swappa.Client.Pages.Components.Account
{
    public partial class UserRegisterDialogue
    {
        private async Task ShowRegisterDialogue()
        {
            var parameters = new ModalParameters
            {
                { "ForSuperUser", false }
            };
            var confirmation = Modal.Show<RegisterDialogue>("", parameters);
            await confirmation.Result;
        }
    }
}
