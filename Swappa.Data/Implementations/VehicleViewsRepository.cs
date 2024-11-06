using Microsoft.Extensions.Options;
using Mongo.Common.MongoDB;
using Mongo.Common.Settings;
using Swappa.Data.Contracts;
using Swappa.Entities.Models;
using System.Linq.Expressions;

namespace Swappa.Data.Implementations
{
    public class VehicleViewsRepository : Repository<VehicleViews>, IVehicleViewsRepository
    {
        public VehicleViewsRepository(IOptions<MongoDbSettings> option)
            : base(option) { }

        public async Task AddAsync(VehicleViews entity) =>
            await CreateAsync(entity);

        public async Task<bool> Exists(Expression<Func<VehicleViews, bool>> expression) =>
            await ExistsAsync(expression);

        public async Task<long> Count(Expression<Func<VehicleViews, bool>> expression) =>
            await CountAsync(expression);
    }
}
