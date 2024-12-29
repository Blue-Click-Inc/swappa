using Blazored.Modal;
using Microsoft.AspNetCore.Components;
using Swappa.Shared.DTOs;

namespace Swappa.Client.Pages.Modals.Accounts
{
    public partial class RegisterDialogue
    {
        [CascadingParameter]
        BlazoredModalInstance Instance { get; set; } = new();
        [Parameter]
        public bool ForSuperUser { get; set; }
        public RegistrationTypeDto RegType { get; set; } = new();

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
        }

        private async Task GoToRegistrationPage()
        {
            var parameters = new ModalParameters
            {
                { "Type", RegType.Type }
            };

            await Instance.CancelAsync();
            var confirm = Modal.Show<RegisterModal>("", parameters);
            await confirm.Result;
        }
    }
}
