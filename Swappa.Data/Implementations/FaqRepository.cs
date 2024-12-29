using Microsoft.Extensions.Options;
using Mongo.Common.MongoDB;
using Mongo.Common.Settings;
using Swappa.Data.Contracts;
using Swappa.Entities.Models;
using System.Linq.Expressions;

namespace Swappa.Data.Implementations
{
    public class FaqRepository : Repository<Faq>, IFaqRepository
    {
        public FaqRepository(IOptions<MongoDbSettings> mongoDbSettings)
            : base(mongoDbSettings) { }

        public async Task AddAsync(Faq entity) =>
            await CreateAsync(entity);

        public async Task AddManyAsync(List<Faq> entities) =>
            await CreateManyAsync(entities);

        public IQueryable<Faq> FindAsQueryable(Expression<Func<Faq, bool>> expression = null!) =>
            expression != null ? GetAsQueryable(expression) : GetAsQueryable();

        public async Task DeleteAsync(Expression<Func<Faq, bool>> predicate) =>
            await RemoveAsync(predicate);

        public async Task<Faq?> FindByIdAsync(Guid id) =>
            await GetAsync(f => f.Id.Equals(id));

        public async Task<bool> Exists(Expression<Func<Faq, bool>> expression) =>
            await ExistsAsync(expression);

        public async Task EditAsync(Expression<Func<Faq, bool>> expression, Faq entity) =>
            await UpdateAsync(expression, entity);

        public async Task<long> Count(Expression<Func<Faq, bool>> expression) =>
            await CountAsync(expression);
    }
}
