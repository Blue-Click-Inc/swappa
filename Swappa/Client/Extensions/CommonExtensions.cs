using Blazored.Modal;
using Blazored.Modal.Services;
using Swappa.Client.Pages.Modals.Vehicle;

namespace Swappa.Client.Extensions
{
    public static class CommonExtensions
    {
        public static async Task<bool> ShowAddVehicleModal(this IModalService modal, Guid userId)
        {
            var idParam = new ModalParameters
            {
                { "UserId", userId }
            };

            var confirmationModal = modal.Show<AddVehicleModal>("", idParam);
            var result = await confirmationModal.Result;
            return result.Confirmed;
        }
    }
}
