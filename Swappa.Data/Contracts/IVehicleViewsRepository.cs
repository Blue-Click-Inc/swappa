using Swappa.Entities.Models;
using System.Linq.Expressions;

namespace Swappa.Data.Contracts
{
    public interface IVehicleViewsRepository
    {
        Task AddAsync(VehicleViews entity);
        Task<long> Count(Expression<Func<VehicleViews, bool>> expression);
        Task<bool> Exists(Expression<Func<VehicleViews, bool>> expression);
    }
}
