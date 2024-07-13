using Swappa.Client.Pages.Modals.Vehicle;

namespace Swappa.Client.Pages.Vehicle
{
    public partial class VehicleAdmin
    {
        private async Task ShowBulkVehicleUploadModal()
        {
            var confirmation = Modal.Show<BulkVehicleUploadModal>("");
            await confirmation.Result;
        }
    }
}
