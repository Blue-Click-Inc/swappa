using Swappa.Entities.Models;
using System.Linq.Expressions;

namespace Swappa.Data.Contracts
{
    public interface ITokenRepository
    {
        Task AddAsync(Token token);
        Task DeleteAsync(Expression<Func<Token, bool>> expression);
        Task<Token?> FindAsync(Expression<Func<Token, bool>> expression);
    }
}
