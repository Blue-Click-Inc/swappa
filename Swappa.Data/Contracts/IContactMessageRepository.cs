using Swappa.Entities.Models;
using System.Linq.Expressions;

namespace Swappa.Data.Contracts
{
    public interface IContactMessageRepository
    {
        Task AddAsync(ContactMessage entity);
        Task DeleteAsync(Expression<Func<ContactMessage, bool>> expression);
        Task EditAsync(Expression<Func<ContactMessage, bool>> expression, ContactMessage entity);
        Task EditManyAsync(List<ContactMessage> entities);
        IQueryable<ContactMessage> FindAsQueryable(Expression<Func<ContactMessage, bool>> expression);
        Task<ContactMessage?> FindByConditionAsync(Expression<Func<ContactMessage, bool>> expression);
        Task<bool> RecordExistsAsync(Expression<Func<ContactMessage, bool>> expression);
        Task<long> RecordsCountAsync(Expression<Func<ContactMessage, bool>> expression);
    }
}
