using Microsoft.AspNetCore.Components;
using Swappa.Shared.DTOs;

namespace Swappa.Client.Pages.Components.Vehicle
{
    public partial class VehicleSearch
    {
        [Parameter]
        public VehicleQueryDto Query { get; set; } = new();
        [Parameter]
        public EventCallback ClearSearch { get; set; }

        protected async override Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
        }
    }
}
