using Hangfire.Console;
using Hangfire.Server;
using Microsoft.Extensions.Configuration;
using RedisCache.Common.Repository;
using Refit;
using Swappa.Data.Contracts;
using Swappa.Data.Services.Interfaces;
using Swappa.Shared.DTOs;

namespace Swappa.Data.Implementations
{
    public class RecurringJobService : IRecurringJobService
    {
        private readonly ICountryService countryService;
        private readonly IStateService stateService;
        private readonly ICacheCommonRepository redisCache;

        public RecurringJobService(IConfiguration configuration,
            ICacheCommonRepository redisCache)
        {
            var baseUrl = configuration.GetSection("LocationService")["BaseUrl"] ?? string.Empty;
            this.countryService = RestService.For<ICountryService>(baseUrl);
            this.stateService = RestService.For<IStateService>(baseUrl);
            this.redisCache = redisCache;
        }

        public async Task CacheLocationData(PerformContext context)
        {
            context.WriteLine("Initializing data");
            var states = new List<StateDataToReturnDto>();

            context.WriteLine("Getting country data from external service");
            var countries = await countryService.Get(1, 10000);
            context.WriteLine($"The response returned {countries.Count} country records");

            context.WriteLine($"Caching {countries.Count} country records into Redis");
            await redisCache.SetAsync<List<CountryDataToReturnDto>>("Countries", countries, DateTimeOffset.MaxValue);

            context.WriteLine($"Getting states records for the {countries.Count} countries");
            foreach (var country in countries)
            {
                context.WriteLine($"Getting states for {country.Name}");
                var stateList = await stateService.Get(country._Id);

                context.WriteLine($"{stateList.Count} states was returned for {country.Name}");
                states.AddRange(stateList);
            }

            context.WriteLine($"Caching {states.Count} states records into Redis");
            await redisCache.SetAsync<List<StateDataToReturnDto>>("States", states, DateTimeOffset.MaxValue);

            context.WriteLine($"{countries.Count} countries and {states.Count} states successfully cached.");
        }
    }
}
