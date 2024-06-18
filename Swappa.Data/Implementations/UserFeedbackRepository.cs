using Microsoft.Extensions.Options;
using Mongo.Common.MongoDB;
using Mongo.Common.Settings;
using Swappa.Data.Contracts;
using Swappa.Entities.Models;
using System.Linq.Expressions;

namespace Swappa.Data.Implementations
{
    public class UserFeedbackRepository : Repository<UserFeedback>, IUserFeedbackRepository
    {
        public UserFeedbackRepository(IOptions<MongoDbSettings> option) 
            : base(option) { }

        public async Task AddAsync(UserFeedback userFeedback) =>
            await CreateAsync(userFeedback);

        public IQueryable<UserFeedback> FindAsQueryable(Expression<Func<UserFeedback, bool>> expression = null!) =>
            expression != null ? GetAsQueryable(expression) : GetAsQueryable();

        public async Task<UserFeedback?> FindAsync(Expression<Func<UserFeedback, bool>> expression) =>
            await GetAsync(expression);

        public async Task DeleteAsync(Expression<Func<UserFeedback, bool>> expression) =>
            await RemoveAsync(expression);

        public async Task EditAsync(Expression<Func<UserFeedback, bool>> expression, UserFeedback feedback) =>
            await UpdateAsync(expression, feedback);

        public async Task<bool> Exists(Expression<Func<UserFeedback, bool>> expression) =>
            await ExistsAsync(expression);
    }
}
