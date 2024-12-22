﻿using BlazorBootstrap;
using Microsoft.AspNetCore.Components;
using Swappa.Shared.DTOs;
using Swappa.Shared.Extensions;

namespace Swappa.Client.Pages.Components
{
    public partial class Favorited
    {
        [Parameter]
        public VehicleToReturnDto Vehicle { get; set; } = new();
        public IconName IconNameType => Vehicle.IsFavorite ?
            IconName.HeartFill : IconName.Heart;

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
        }

        private async Task<bool> IsFavoriteVehicle(Guid id)
        {
            var response = await VehicleService.IsFavorite(id);
            if (response.IsSuccessful)
            {
                return response.Data;
            }

            return false;
        }

        private async Task ToggleFavorite()
        {
            var response = await VehicleService.ToggleFavorite(new Swappa.Shared.DTOs.IdDto
            {
                Id = Vehicle.Id
            });

            if(response.IsNotNull() && response.IsSuccessful)
            {
                await LocalStorage.SetItemAsync("favoriteVehicles", response.Data.AllFavorites);
                Vehicle.IsFavorite = response.Data.IsFavorite;
                Vehicle.Favorited = response.Data.Favorites;
            }
        }
    }
}
