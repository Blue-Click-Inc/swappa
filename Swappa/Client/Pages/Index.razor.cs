using Swappa.Shared.DTOs;

namespace Swappa.Client.Pages
{
    public partial class Index
    {
        private bool isLoading = false;
        private string message = string.Empty;

        public VehicleQueryDto Query { get; set; } = new VehicleQueryDto();
        public PaginatedListDto<VehicleToReturnDto>? Vehicles { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await GetDataAsync();
            await base.OnInitializedAsync();
        }

        private async Task GetDataAsync()
        {
            isLoading = Vehicles == null;
            var response = await VehicleService.GetDataAsync(Query);
            if (response != null && response.IsSuccessful)
            {
                Vehicles = response.Data;
            }
            else
            {
                message = response?.Message ?? string.Empty;
            }
            isLoading = false;
        }
    }
}
