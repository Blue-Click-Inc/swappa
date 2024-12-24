using Blazored.Modal;
using Swappa.Client.Pages.Modals.User;
using Swappa.Shared.Extensions;
using System.Runtime.CompilerServices;

namespace Swappa.Client.Shared
{
    public partial class Contact
    {
        protected async override Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
        }

        private async Task LeaveAFeedback()
        {
            var token = await LocalStorage.GetItemAsync<string>("accessToken");
            var email = AccountService
                .GetUserEmailFromToken(token);

            if (email.IsNotNullOrEmpty())
            {
                var emailParam = new ModalParameters
                {
                    { "Email", email }
                };

                var confirmationModal = Modal.Show<UserFeedbackModal>("", emailParam);
                await confirmationModal.Result;
            }
        }
    }
}
