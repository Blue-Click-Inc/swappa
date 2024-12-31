using Microsoft.AspNetCore.Components;
using Swappa.Client.Extensions;
using Swappa.Shared.DTOs;
using Swappa.Shared.Extensions;

namespace Swappa.Client.Pages.Components.Vehicle
{
    public partial class VehiclesTableListWithSearch
    {
        [Parameter]
        public EventCallback GetData { get; set; }
        [Parameter]
        public PaginatedListDto<VehicleToReturnDto>? Data { get; set; }
        [Parameter]
        public VehicleQueryDto Query { get; set; } = new();
        [Parameter]
        public Guid UserId { get; set; }
        [Parameter]
        public bool IsLoading { get; set; }
        public string NumberOfVehicles => Data.IsNull() ?
            0.0m.ToString() :
            Data.MetaData.TotalCount.ToString("#,##");

        protected override async Task OnInitializedAsync()
        {
            await GetDataAsync();
            await base.OnInitializedAsync();
        }

        private async Task GetDataAsync()
        {
            await GetData.InvokeAsync();
        }

        private async Task ShowAddVehicleModal()
        {
            var confirmed = await Modal.ShowAddVehicleModal(UserId);
            if (confirmed)
            {
                await GetDataAsync();
            }
        }

        private async Task OnPageChangedAsync(int newPageNumber)
        {
            Query.PageNumber = newPageNumber;
            await GetDataAsync();
        }

        private async Task Search()
        {
            await GetDataAsync();
        }

        private async Task Clear()
        {
            Query = new();
            await GetDataAsync();
        }
    }
}