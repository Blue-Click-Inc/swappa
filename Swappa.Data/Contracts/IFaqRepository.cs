using Swappa.Entities.Models;
using System.Linq.Expressions;

namespace Swappa.Data.Contracts
{
    public interface IFaqRepository
    {
        Task AddAsync(Faq entity);
        Task AddManyAsync(List<Faq> entities);
        Task<long> Count(Expression<Func<Faq, bool>> expression);
        Task DeleteAsync(Expression<Func<Faq, bool>> predicate);
        Task EditAsync(Expression<Func<Faq, bool>> expression, Faq entity);
        Task<bool> Exists(Expression<Func<Faq, bool>> expression);
        IQueryable<Faq> FindAsQueryable(Expression<Func<Faq, bool>> expression = null!);
        Task<Faq?> FindByIdAsync(Guid id);
    }
}
