using BlazorBootstrap;
using Microsoft.AspNetCore.Components;
using Swappa.Shared.DTOs;

namespace Swappa.Client.Pages.Components.Vehicle
{
    public partial class VehicleFilter
    {
        private bool _showFilter = true;

        [Parameter]
        public VehicleQueryDto Query { get; set; } = new();
        public string FilterClass => _showFilter ? "display-filter" : null!;
        public IconName FilterIcon => _showFilter ? IconName.Filter : IconName.EyeSlash;

        protected async override Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
        }

        private void ToggleFilterDisplay()
        {
            _showFilter = !_showFilter;
        }
    }
}
