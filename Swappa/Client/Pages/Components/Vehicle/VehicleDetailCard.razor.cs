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
        public Dictionary<string, string> Urls { get; set; } = new();
        public bool Show { get; set; } 

        protected override async Task OnParametersSetAsync()
        {
            Show = await IsTheOwner();
            MakeUrlDictionary();
            await base.OnParametersSetAsync();
        }

        private async Task<bool> IsTheOwner()
        {
            var userIdString = await LocalStorage.GetItemAsStringAsync("userId");
            return userIdString.IsNotNullOrEmpty() && Data.UserId.Equals(userIdString.Replace("\"", "").ToGuid());
        }

        private void MakeUrlDictionary()
        {
            if(Data.IsNotNull() && Data.Images.IsNotNullOrEmpty())
            {
                Urls = Data.Images.ToDictionary(key => key.Url, value => $"{Data.Make} {Data.Model} {Data.Trim} {Data.Year}");
            }
        }
    }
}
