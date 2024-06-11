using Swappa.Entities.Models;
using System.Linq.Expressions;

namespace Swappa.Data.Contracts
{
    public interface IUserFeedbackRepository
    {
        Task AddAsync(UserFeedback userFeedback);
        Task DeleteAsync(Expression<Func<UserFeedback, bool>> expression);
        IQueryable<UserFeedback> FindAsQueryable(Expression<Func<UserFeedback, bool>> expression = null!);
        Task<UserFeedback?> FindAsync(Expression<Func<UserFeedback, bool>> expression);
    }
}
