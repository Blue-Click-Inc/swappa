using Swappa.Entities.Models;
using System.Linq.Expressions;

namespace Swappa.Data.Contracts
{
    public interface ILocationService
    {
        Task AddAsync(EntityLocation entity);
        Task EditAsync(Expression<Func<EntityLocation, bool>> expression, EntityLocation entity);
        Task DeleteAsync(Expression<Func<EntityLocation, bool>> expression);
        Task<EntityLocation?> FindOneAsync(Expression<Func<EntityLocation, bool>> expression);
        Task<bool> Exists(Expression<Func<EntityLocation, bool>> expression);
        Task AddAsync(List<EntityLocation> entities);
        Task<List<EntityLocation>> FindManyAsync(Expression<Func<EntityLocation, bool>> expression);
    }
}
