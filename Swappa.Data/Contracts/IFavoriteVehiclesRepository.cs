using Swappa.Entities.Models;
using System.Linq.Expressions;

namespace Swappa.Data.Contracts
{
    public interface IFavoriteVehiclesRepository
    {
        Task AddAsync(FavoriteVehicles entity);
        Task<long> Count(Expression<Func<FavoriteVehicles, bool>> expression);
        Task DeleteAsync(Expression<Func<FavoriteVehicles, bool>> predicate);
        Task<bool> Exists(Expression<Func<FavoriteVehicles, bool>> expression);
        IQueryable<FavoriteVehicles> FindAsQueryable(Expression<Func<FavoriteVehicles, bool>> expression = null);
    }
}
