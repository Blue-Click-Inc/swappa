using Microsoft.AspNetCore.Components;
using Swappa.Entities.Enums;

namespace Swappa.Client.Pages.Modals.Accounts
{
    public partial class RegisterModal
    {
        [Parameter]
        public SystemRole Type { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
        }
    }
}
