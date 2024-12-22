using Blazored.Modal;
using Swappa.Client.Pages.Modals.Accounts;
using Swappa.Entities.Enums;

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
            var parameters = new ModalParameters
            {
                { "Type", SystemRole.User }
            };
            var confirmation = Modal.Show<RegisterModal>("", parameters);
            await confirmation.Result;
        }
    }
}
