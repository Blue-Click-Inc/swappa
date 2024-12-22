using Swappa.Client.Pages.Modals.Accounts;
using Swappa.Shared.Extensions;

namespace Swappa.Client.Shared
{
    public partial class MainLayout
    {
        public string Favorites { get; set; } = "0";
        protected async override Task OnInitializedAsync()
        {
            await GetFavoriteVehicleCount();
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

        private async Task GetFavoriteVehicleCount()
        {
            var favorites = await LocalStorage.GetItemAsync<long>("favoriteVehicles");
            Favorites = favorites.ToShortNumString();
        }
    }
}
