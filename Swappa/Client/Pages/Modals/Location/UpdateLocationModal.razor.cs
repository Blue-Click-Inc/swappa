using Blazored.Modal;
using Microsoft.AspNetCore.Components;
using Swappa.Entities.Enums;
using Swappa.Shared.DTOs;
using Swappa.Shared.Extensions;

namespace Swappa.Client.Pages.Modals.Location
{
    public partial class UpdateLocationModal
    {
        private bool isLoading = false;
        private bool initiallyLoading = false;
        private string buttonLabel = "Save";
        private string pageTitle = "Update Location";

        [CascadingParameter]
        BlazoredModalInstance Instance { get; set; } = new();
        [Parameter]
        public Guid EntityId { get; set; }
        [Parameter]
        public EntityType EntityType { get; set; }
        public BaseLocationDto? Location { get; set; }
        public List<CountryData>? Countries { get; set; }
        public List<StateData>? States { get; set; }
        public ResponseModel<string>? Response { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await InitializeObjectAsync();
            await base.OnInitializedAsync();
        }

        private async Task InitializeObjectAsync()
        {
            if (EntityId.IsNotEmpty())
            {
                initiallyLoading = Location == null || Countries == null || States == null;
                var locationResponse = await LocationService.GetAsync(EntityId);
                if (locationResponse != null && locationResponse.IsSuccessful)
                {
                    Location = locationResponse.Data;
                    var countryResponse = await LocationsService.GetCountriesAsync();
                    if (countryResponse != null)
                    {
                        Countries = countryResponse.Data;
                        var stateResponse = await LocationsService.GetStatesAsync(Location.CountryId.ToGuid());
                        if (stateResponse != null)
                        {
                            States = stateResponse;
                        }
                    }
                    else
                    {
                        Toast.ShowError("Something went wrong. Please try again later.");
                    }
                }
                else
                {
                    Toast.ShowError(locationResponse?.Message ?? "Something went wrong. Please try again later.");
                }
                initiallyLoading = false;
            }
            else
            {
                Toast.ShowError("Invalid entity Id. Please try again");
            }
        }

        private async Task OnCountrySelected(ChangeEventArgs e)
        {
            Location.StateId = string.Empty;
            Location.City = string.Empty;
            Location.PostalCode = string.Empty;
            States = new List<StateData>();

            if (!string.IsNullOrWhiteSpace(e.Value.ToString()))
            {
                Location.CountryId = e.Value.ToString() ?? string.Empty;

                var response = await LocationsService.GetStatesAsync(Location.CountryId.ToGuid());
                if (response != null)
                {
                    States = response;
                }
                else
                {
                    Toast.ShowError("An error occurred. Please try again later.");
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

        private async Task UpdateAsync()
        {
            if(Location != null)
            {
                if (Response == null)
                {
                    isLoading = true;
                }

                Response = await LocationService.UpdateAsync(Location);
                if (Response != null)
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

        private async Task DeleteAsync(Guid entityId)
        {
            if(entityId.IsNotEmpty())
            {
                isLoading = true;

                Response = await LocationService.DeleteAsync(entityId);
                if (Response != null)
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
}
