using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Mongo.Common.MongoDB;
using Mongo.Common.Settings;
using Refit;
using Swappa.Data.Contracts;
using Swappa.Data.Services.Interfaces;
using Swappa.Entities.Models;
using Swappa.Shared.DTOs;
using System.Linq.Expressions;

namespace Swappa.Data.Implementations
{
    public class LocationService : Repository<EntityLocation>, ILocationService
    {
        private readonly ICountryService countryService;
        private readonly IStateService stateService;

        public LocationService(IConfiguration configuration, IOptions<MongoDbSettings> option) : base(option)
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

        public async Task AddAsync(CountryDataDto request)
        {
            await countryService.Post(request);
        }

        public async Task EditAsync(string countryId, CountryDataDto request)
        {
            await countryService.Put(countryId, request);
        }

        public async Task DeleteAsync(string countryId)
        {
            await countryService.Delete(countryId);
        }

        #endregion

        #region Entity Location Section
        public async Task AddAsync(EntityLocation entity) =>
            await CreateAsync(entity);

        public async Task EditAsync(Expression<Func<EntityLocation, bool>> expression, EntityLocation entity) =>
            await UpdateAsync(expression, entity);

        public async Task DeleteAsync(Expression<Func<EntityLocation, bool>> expression) =>
            await RemoveAsync(expression);

        public async Task<EntityLocation?> GetByConditionAsync(Expression<Func<EntityLocation, bool>> expression) =>
            await GetAsync(expression);

        public async Task<bool> Exists(Expression<Func<EntityLocation, bool>> expression) =>
            await ExistsAsync(expression);
        #endregion
    }
}
