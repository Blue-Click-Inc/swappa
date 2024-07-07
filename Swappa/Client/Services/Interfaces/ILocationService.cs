﻿using Swappa.Shared.DTOs;

namespace Swappa.Client.Services.Interfaces
{
    public interface ILocationService
    {
        Task<ResponseModel<string>?> AddAsync(BaseLocationDto request);
        Task<ResponseModel<string>?> DeleteAsync(Guid entityId);
        Task<ResponseModel<BaseLocationDto>?> GetAsync(Guid entityId);
        Task<ResponseModel<List<CountryDataToReturnDto>>?> GetCountriesAsync();
        Task<ResponseModel<List<StateDataToReturnDto>>?> GetStatesAsync(string countryId);
        Task<ResponseModel<string>?> UpdateAsync(BaseLocationDto request);
    }
}