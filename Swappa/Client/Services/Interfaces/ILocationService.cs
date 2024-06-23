using Swappa.Shared.DTOs;

namespace Swappa.Client.Services.Interfaces
{
    public interface ILocationService
    {
        Task<ResponseModel<string>?> AddAsync(BaseLocationDto request);
        Task<ResponseModel<List<CountryDataToReturnDto>>?> GetCountriesAsync();
        Task<ResponseModel<List<StateDataToReturnDto>>?> GetStatesAsync(string countryId);
        Task<ResponseModel<string>?> UpdateAsync(BaseLocationDto request);
    }
}
