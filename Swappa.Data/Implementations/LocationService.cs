using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Mongo.Common.MongoDB;
using Mongo.Common.Settings;
using RedisCache.Common.Repository;
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
        private readonly ICacheCommonRepository redisCache;

        public LocationService(IConfiguration configuration, 
            IOptions<MongoDbSettings> option,
            ICacheCommonRepository redisCache) 
            : base(option)
        {
            var baseUrl = configuration.GetSection("LocationService")["BaseUrl"] ?? string.Empty;
            this.countryService = RestService.For<ICountryService>(baseUrl);
            this.stateService = RestService.For<IStateService>(baseUrl);
            this.redisCache = redisCache;
        }

        #region State Section
        public async Task<List<StateDataToReturnDto>> GetManyAsync(string countryId)
        {
            var states = new List<StateDataToReturnDto>();

            if (redisCache.KeyExists("States"))
            {
                var cachedData = await redisCache.GetAsync<List<StateDataToReturnDto>>("States");
                states = cachedData
                    .OrderBy(s => s.Name)
                    .Where(s => s.Country.Equals(countryId))
                    .ToList();
            }
            else
            {
                states = await stateService.Get(countryId);
                await redisCache.SetAsync<List<StateDataToReturnDto>>("States", states, DateTimeOffset.MaxValue);
            }

            return states;
        }

        public async Task<StateDataToReturnDto?> GetOneAsync(string id)
        {
            var data = new StateDataToReturnDto();
            if (redisCache.KeyExists("States"))
            {
                var list = await redisCache.GetAsync<List<StateDataToReturnDto>>("States");
                data = list.FirstOrDefault(s => s._Id.Equals(id));
            }
            else
            {
                data = await stateService.GetOne(id);
            }

            return data;
        }
        #endregion

        #region Country Section
        public async Task<List<CountryDataToReturnDto>> GetAsync(int page, int pageSize)
        {
            var countries = new List<CountryDataToReturnDto>();

            if (redisCache.KeyExists("Countries"))
            {
                var cachedData = await redisCache.GetAsync<List<CountryDataToReturnDto>>("Countries");
                countries = cachedData
                    .OrderBy(c => c.Name)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();
            }
            else
            {
                countries = await countryService.Get(page, pageSize);
                await redisCache.SetAsync<List<CountryDataToReturnDto>>("Countries", countries, DateTimeOffset.MaxValue);
            }
            
            return countries;
        }

        public async Task<CountryDataToReturnDto?> GetAsync(string countryId)
        {
            var data = new CountryDataToReturnDto();
            if (redisCache.KeyExists($"Countries"))
            {
                var list = await redisCache.GetAsync<List<CountryDataToReturnDto>>($"Countries");
                data = list.FirstOrDefault(c => c._Id.Equals(countryId));
            }
            else
            {
                data = await countryService.Get(countryId);
            }
            
            return data;
        }

        public async Task AddAsync(CountryDataDto request)
        {
            await countryService.Post(request);
            await redisCache.RemoveAsync("Countries");
        }

        public async Task EditAsync(string countryId, CountryDataDto request)
        {
            await countryService.Put(countryId, request);
            await redisCache.RemoveAsync("Countries");
        }

        public async Task DeleteAsync(string countryId)
        {
            await countryService.Delete(countryId);
            await redisCache.RemoveAsync("Countries");
        }

        #endregion

        #region Location Entity Section
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
