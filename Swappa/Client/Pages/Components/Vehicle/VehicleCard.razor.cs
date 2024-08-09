using Microsoft.AspNetCore.Components;
using Swappa.Shared.DTOs;

namespace Swappa.Client.Pages.Components.Vehicle
{
    public partial class VehicleCard
    {
        [Parameter]
        public VehicleToReturnDto Vehicle { get; set; } = new();
    }
}
