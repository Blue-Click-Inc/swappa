using Microsoft.AspNetCore.Components;
using Swappa.Shared.DTOs;

namespace Swappa.Client.Pages.Components.Vehicle
{
    public partial class VehicleTableList
    {
        [Parameter]
        public PaginatedListDto<VehicleToReturnDto>? Data { get; set; }
    }
}
