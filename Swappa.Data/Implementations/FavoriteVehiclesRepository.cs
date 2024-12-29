using Microsoft.Extensions.Options;
using Mongo.Common.MongoDB;
using Mongo.Common.Settings;
using Swappa.Data.Contracts;
using Swappa.Entities.Models;
using System.Linq.Expressions;

namespace Swappa.Data.Implementations
{
    public class FavoriteVehiclesRepository : Repository<FavoriteVehicles>, IFavoriteVehiclesRepository
    {
        public FavoriteVehiclesRepository(IOptions<MongoDbSettings> mongoDbSettings) 
            : base(mongoDbSettings) {}

        public async Task AddAsync(FavoriteVehicles entity) =>
            await CreateAsync(entity);

        public async Task DeleteAsync(Expression<Func<FavoriteVehicles, bool>> predicate) =>
            await RemoveAsync(predicate);

        public IQueryable<FavoriteVehicles> FindAsQueryable(Expression<Func<FavoriteVehicles, bool>> expression = null!) =>
            expression != null ? GetAsQueryable(expression) : GetAsQueryable();

        public async Task<bool> Exists(Expression<Func<FavoriteVehicles, bool>> expression) =>
            await ExistsAsync(expression);

        public async Task<long> Count(Expression<Func<FavoriteVehicles, bool>> expression) =>
            await CountAsync(expression);
    }
}
