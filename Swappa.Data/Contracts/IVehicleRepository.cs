using Swappa.Entities.Models;
using System.Linq.Expressions;

namespace Swappa.Data.Contracts
{
    public interface IVehicleRepository
    {
        Task AddAsync(Vehicle entity);
        Task AddAsync(List<Vehicle> vehicles);
        Task<long> Count(Expression<Func<Vehicle, bool>> expression);
        Task DeleteAsync(Expression<Func<Vehicle, bool>> expression);
        Task EditAsync(Expression<Func<Vehicle, bool>> expression, Vehicle entity);
        Task<bool> Exists(Expression<Func<Vehicle, bool>> expression);
        IQueryable<Vehicle> FindAsQueryable(Expression<Func<Vehicle, bool>> expression = null!);
        Task<Vehicle?> FindAsync(Expression<Func<Vehicle, bool>> expression);
    }
}
