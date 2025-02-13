using Microsoft.Extensions.Options;
using Mongo.Common.MongoDB;
using Mongo.Common.Settings;
using Swappa.Data.Contracts;
using Swappa.Entities.Models;
using System.Linq.Expressions;

namespace Swappa.Data.Implementations
{
    public class LocationService : Repository<EntityLocation>, ILocationService
    {
        public LocationService(IOptions<MongoDbSettings> option) 
            : base(option)
        {           
        }

        #region Location Entity Section
        public async Task AddAsync(EntityLocation entity) =>
            await CreateAsync(entity);

        public async Task AddAsync(List<EntityLocation> entities) =>
            await CreateManyAsync(entities);

        public async Task EditAsync(Expression<Func<EntityLocation, bool>> expression, EntityLocation entity) =>
            await UpdateAsync(expression, entity);

        public async Task DeleteAsync(Expression<Func<EntityLocation, bool>> expression) =>
            await RemoveAsync(expression);

        public async Task<EntityLocation?> FindOneAsync(Expression<Func<EntityLocation, bool>> expression) =>
            await GetAsync(expression);

        public async Task<List<EntityLocation>> FindManyAsync(Expression<Func<EntityLocation, bool>> expression) =>
            await GetManyAsync(expression);

        public async Task<bool> Exists(Expression<Func<EntityLocation, bool>> expression) =>
            await ExistsAsync(expression);
        #endregion
    }
}
