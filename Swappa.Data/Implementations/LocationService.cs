using Microsoft.Extensions.Configuration;
using Refit;
using Swappa.Data.Contracts;
using Swappa.Data.Services.Interfaces;
using Swappa.Shared.DTOs;

namespace Swappa.Data.Implementations
{
    public class LocationService : ILocationService
    {
        private readonly ICountryService countryService;
        private readonly IStateService stateService;

        public LocationService(IConfiguration configuration)
        {
            var baseUrl = configuration.GetSection("LocationService")["BaseUrl"] ?? string.Empty;
            this.countryService = RestService.For<ICountryService>(baseUrl);
            this.stateService = RestService.For<IStateService>(baseUrl);
        }

        #region State Section
        public async Task<List<StateDataToReturnDto>> GetManyAsync(string countryId)
        {
            var data = await stateService.Get(countryId);
            return data;
        }

        public async Task<StateDataToReturnDto> GetOneAsync(string id)
        {
            var data = await stateService.GetOne(id);
            return data;
        }
        #endregion

        #region Country Section
        public async Task<List<CountryDataToReturnDto>> GetAsync(int page, int pageSize)
        {
            var list = await countryService.Get(page, pageSize);
            return list;
        }

        public async Task<CountryDataToReturnDto> GetAsync(string countryId)
        {
            var data = await countryService.Get(countryId);
            return data;
        }

        public async Task CreateAsync(CountryDataDto request)
        {
            await countryService.Post(request);
        }

        public async Task UpdateAsync(string countryId, CountryDataDto request)
        {
            await countryService.Put(countryId, request);
        }

        public async Task DeleteAsync(string countryId)
        {
            await countryService.Delete(countryId);
        }

        #endregion
    }
}
