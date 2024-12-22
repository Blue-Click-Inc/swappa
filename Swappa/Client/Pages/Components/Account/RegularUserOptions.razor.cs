using Blazored.Modal;
using Microsoft.AspNetCore.Components;
using Swappa.Client.Pages.Modals.Accounts;
using Swappa.Shared.DTOs;

namespace Swappa.Client.Pages.Components.Account
{
    public partial class RegularUserOptions
    {
        [Parameter]
        public RegistrationTypeDto RegType { get; set; } = new();

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
        }
    }
}
