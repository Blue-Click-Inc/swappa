using Swappa.Client.Pages.Modals.Accounts;
using Swappa.Client.State;
using Swappa.Shared.Extensions;

namespace Swappa.Client.Shared
{
    public partial class MainLayout
    {
        public string Favorites { get; set; } = "0";
        protected async override Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();   
        }

        private async Task ShowLoginModal()
        {
            var confirmationModal = Modal.Show<LoginModal>("");
            var result = await confirmationModal.Result;
        }

        private async Task ShowRegisterModal()
        {
            var confirmation = Modal.Show<RegisterModal>("");
            await confirmation.Result;
        }
    }
}
