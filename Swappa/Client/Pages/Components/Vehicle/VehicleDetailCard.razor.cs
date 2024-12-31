using BlazorBootstrap;
using Blazored.Modal;
using Microsoft.AspNetCore.Components;
using Swappa.Client.Pages.Modals;
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
        public ConfirmDialog Dialog { get; set; } = new();

        protected override async Task OnParametersSetAsync()
        {
            Show = await IsTheOwner();
            MakeUrlDictionary();
            await base.OnParametersSetAsync();
        }

        private async Task ProcessPayment()
        {
            await Task.Run(() =>
                Toast.ShowInfo("Feature coming soon. Watch out!!!")
            );
        }
        private async Task ConfirmDelete()
        {
            var parameters = SharedService.GetDialogParameters("Confirm Delete",
                "This vehicle record will be permanently deleted. Do you wish to proceed?");

            var dialog = Modal.Show<ConfirmationDialog>(parameters);
            var result = await dialog.Result;
            if(result.Confirmed)
            {

            }
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
