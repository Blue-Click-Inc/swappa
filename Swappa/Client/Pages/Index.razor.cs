using BlazorBootstrap;
using Swappa.Shared.DTOs;
using Swappa.Shared.Extensions;

namespace Swappa.Client.Pages
{
    public partial class Index
    {
        private bool _isLoading = false;
        private string _message = string.Empty;
        private readonly string _pageTitle = $"{Statics.AppName} - Home";
        private bool _showFilter = true;

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
                _message = response?.Message ?? string.Empty;
            }
            _isLoading = false;
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
