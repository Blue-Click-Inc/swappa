using Microsoft.Extensions.Options;
using Mongo.Common.MongoDB;
using Mongo.Common.Settings;
using Swappa.Data.Contracts;
using Swappa.Entities.Models;
using System.Linq.Expressions;

namespace Swappa.Data.Implementations
{
    public class VehicleRepository : Repository<Vehicle>, IVehicleRepository
    {
        public VehicleRepository(IOptions<MongoDbSettings> option)
            : base(option) { }

        public async Task AddAsync(Vehicle entity) =>
            await CreateAsync(entity);

        public async Task AddAsync(List<Vehicle> vehicles) =>
            await CreateManyAsync(vehicles);

        public IQueryable<Vehicle> FindAsQueryable(Expression<Func<Vehicle, bool>> expression = null!) =>
            expression != null ? GetAsQueryable(expression) : GetAsQueryable();

        public async Task<Vehicle?> FindAsync(Expression<Func<Vehicle, bool>> expression) =>
            await GetAsync(expression);

        public async Task DeleteAsync(Expression<Func<Vehicle, bool>> expression) =>
            await RemoveAsync(expression);

        public async Task EditAsync(Expression<Func<Vehicle, bool>> expression, Vehicle entity) =>
            await UpdateAsync(expression, entity);

        public async Task<bool> Exists(Expression<Func<Vehicle, bool>> expression) =>
            await ExistsAsync(expression);

        public async Task<long> Count(Expression<Func<Vehicle, bool>> expression) =>
            await CountAsync(expression);
    }
}
