using Microsoft.AspNetCore.Components;
using Swappa.Shared.DTOs;

namespace Swappa.Client.Pages.Components.Vehicle
{
    public partial class VehicleDetailCard
    {
        private bool _isBusy = false;

        [Parameter]
        public VehicleToReturnDto Data { get; set; } = new();

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
        }
    }
}
