using Blazored.Modal;
using Microsoft.AspNetCore.Components;
using Swappa.Shared.DTOs;

namespace Swappa.Client.Pages.Modals.Location
{
    public partial class AddLocationModal
    {
        private bool isLoading = false;
        private string buttonLabel = "Save";
        private string pageTitle = "Add Location";

        [CascadingParameter]
        BlazoredModalInstance Instance { get; set; } = new();
        [Parameter]
        public Guid EntityId { get; set; }
        public BaseLocationDto Location { get; set; } = new();
        public List<CountryDataToReturnDto>? Countries { get; set; } = new();
        public List<StateDataToReturnDto>? States { get; set; } = new();
        public ResponseModel<string>? Response { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var countryResponse = await LocationService.GetCountriesAsync();
            if(countryResponse != null)
            {
                if (countryResponse.IsSuccessful)
                {
                    Countries = countryResponse.Data;
                }
                else
                {
                    Toast.ShowError(countryResponse?.Message ?? "Something went wrong. Please try again later.");
                }
            }
            Location.EntityId = EntityId;
            await base.OnInitializedAsync();
        }

        private async Task OnCountrySelected(ChangeEventArgs e)
        {
            Location.StateId = string.Empty;
            Location.City = string.Empty;
            Location.PostalCode = string.Empty;
            States = new List<StateDataToReturnDto>();

            if (!string.IsNullOrWhiteSpace(e.Value.ToString()))
            {
                Location.CountryId = e.Value.ToString() ?? string.Empty;

                var response = await LocationService.GetStatesAsync(Location.CountryId);
                if(response != null)
                {
                    if (response.IsSuccessful)
                    {
                        States = response.Data;
                    }
                    else
                    {
                        Toast.ShowError(response.Message ?? "An error occurred. Please try again later.");
                    }
                }
            }
            else
            {
                Toast.ShowError("Ivalid country value selected. Please try again.");
            }
            this.StateHasChanged();
        }

        private void OnStateSelected(ChangeEventArgs e)
        {
            Location.City = string.Empty;
            Location.PostalCode = string.Empty;
            if (!string.IsNullOrWhiteSpace(e.Value.ToString()))
            {
                Location.StateId = e.Value.ToString() ?? string.Empty;
            }
            this.StateHasChanged();
        }

        private async Task SaveAsync()
        {
            if(Response == null)
            {
                isLoading = true;
            }

            Response = await LocationService.AddAsync(Location);
            if(Response != null)
            {
                if (Response.IsSuccessful)
                {
                    Toast.ShowSuccess(Response.Data ?? string.Empty);
                    await Instance.CloseAsync();
                    return;
                }
                else
                {
                    Toast.ShowError(Response?.Message ?? "Something went wrong. Please try again.");
                }
            }

            isLoading = false;
        }
    }
}
