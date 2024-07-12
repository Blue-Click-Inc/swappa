using Swappa.Entities.Models;
using System.Linq.Expressions;

namespace Swappa.Data.Contracts
{
    public interface IImageRepository
    {
        Task AddAsync(Image image);
        Task AddAsync(ICollection<Image> images);
        Task AddAsync(List<Image> images);
        Task<long> Count(Expression<Func<Image, bool>> expression);
        Task DeleteAsync(Expression<Func<Image, bool>> expression);
        Task EditAsync(Expression<Func<Image, bool>> expression, Image image);
        Task<bool> Exists(Expression<Func<Image, bool>> expression);
        IQueryable<Image> FindAsQueryable(Expression<Func<Image, bool>> expression);
        IQueryable<Image> FindAsQueryable();
        Task<Image?> FindAsync(Expression<Func<Image, bool>> expression);
        Task<List<Image>> FindManyAsync(Expression<Func<Image, bool>> expression);
    }
}
