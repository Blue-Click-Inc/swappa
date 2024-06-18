using Swappa.Entities.Models;
using System.Linq.Expressions;

namespace Swappa.Data.Contracts
{
    public interface IUserFeedbackRepository
    {
        Task AddAsync(UserFeedback userFeedback);
        Task<long> Count(Expression<Func<UserFeedback, bool>> expression);
        Task DeleteAsync(Expression<Func<UserFeedback, bool>> expression);
        Task EditAsync(Expression<Func<UserFeedback, bool>> expression, UserFeedback feedback);
        Task<bool> Exists(Expression<Func<UserFeedback, bool>> expression);
        IQueryable<UserFeedback> FindAsQueryable(Expression<Func<UserFeedback, bool>> expression = null!);
        Task<UserFeedback?> FindAsync(Expression<Func<UserFeedback, bool>> expression);
    }
}
