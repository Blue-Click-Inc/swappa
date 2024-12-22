using Microsoft.AspNetCore.Components;
using Swappa.Shared.DTOs;

namespace Swappa.Client.Pages.Vehicle
{
    public partial class VehicleDetail
    {
        private bool _isBusy = false;
        private bool _isError = false;
        private string _message = string.Empty;
        private string _pageTitle = string.Empty;
        private string _messageHeader = string.Empty;

        [Parameter]
        public Guid Id { get; set; }
        public VehicleToReturnDto? Data { get; set; }
        
        protected async override Task OnParametersSetAsync()
        {
            await GetResponseAsync(Id);
            
            await base.OnParametersSetAsync();
        }

        private async Task GetResponseAsync(Guid id)
        {
            _isBusy = true;
            var response = await VehicleService.GetByIdAsync(id);
            if (response == null)
            {
                _isError = true;
                _message = "An error occurred. Please contact the System Administrator if the issue persists";
                _messageHeader = "Unexpected An error occurred";
            }
            else
            {
                if (response.IsSuccessful)
                {
                    Data = response.Data;
                    _pageTitle = $"{Data.Make} {Data.Model} {Data.Trim} {Data.Year}";
                    _isError = false;
                }
                else
                {
                    _isError = true;
                    _message = response.Message ?? "An error occurred. Please contact the System Administrator if the issue persists";
                    _messageHeader = response.StatusCode == 404 ? "Not found" : "An error occurred";
                }
            }

            _isBusy = false;
            return;
        }
    }
}
