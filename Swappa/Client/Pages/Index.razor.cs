using BlazorBootstrap;
using Swappa.Shared.DTOs;
using Swappa.Shared.Extensions;

namespace Swappa.Client.Pages
{
    public partial class Index
    {
        private bool _isLoading = false;
        private readonly string _pageTitle = $"{Statics.AppName} - Home";
        private bool _showFilter = true;
        private bool _hasError = false;

        public IconName FilterIcon => _showFilter ? IconName.Filter : IconName.EyeSlash;
        public string FilterClass => _showFilter ? "display-filter" : null!;
        public VehicleQueryDto Query { get; set; } = new();
        public PaginatedListDto<VehicleToReturnDto>? Vehicles { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await GetDataAsync();
            await base.OnInitializedAsync();
        }

        private async Task GetDataAsync()
        {
            _isLoading = Vehicles == null;
            var response = await VehicleService.GetDataAsync(Query);
            if (response is { IsSuccessful: true })
            {
                Vehicles = response.Data;
            }
            else
            {
                _hasError = true;
            }
            _isLoading = false;
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
