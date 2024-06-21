using Swappa.Shared.DTOs;

namespace Swappa.Data.Contracts
{
    public interface ILocationService
    {
        Task CreateAsync(CountryDataDto request);
        Task DeleteAsync(string countryId);
        Task<List<CountryDataToReturnDto>> GetAsync(int page, int pageSize);
        Task<CountryDataToReturnDto> GetAsync(string countryId);
        Task<List<StateDataToReturnDto>> GetManyAsync(string countryId);
        Task<StateDataToReturnDto> GetOneAsync(string id);
        Task UpdateAsync(string countryId, CountryDataDto request);
    }
}
