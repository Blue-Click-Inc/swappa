using Microsoft.AspNetCore.Components;
using Swappa.Shared.DTOs;
using Swappa.Shared.Extensions;

namespace Swappa.Client.Pages.Components.Vehicle
{
    public partial class VehicleDetailCard
    {
        private bool _isBusy = false;

        [Parameter]
        public VehicleToReturnDto Data { get; set; } = new();
        public bool Show { get; set; } 

        protected override async Task OnParametersSetAsync()
        {
            Show = await IsTheOwner();
            await base.OnParametersSetAsync();
        }

        private async Task<bool> IsTheOwner()
        {
            var userIdString = await LocalStorage.GetItemAsStringAsync("userId");
            var userId = userIdString.Replace("\"", "").ToGuid();
            return Data.UserId.Equals(userId);
        }
    }
}
