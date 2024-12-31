using Microsoft.Extensions.Options;
using Mongo.Common.MongoDB;
using Mongo.Common.Settings;
using Swappa.Data.Contracts;
using Swappa.Entities.Models;
using System.Linq.Expressions;

namespace Swappa.Data.Implementations
{
    public class ContactMessageRepository : Repository<ContactMessage>, IContactMessageRepository
    {
        public ContactMessageRepository(IOptions<MongoDbSettings> options) 
            : base(options)
        {}

        public async Task AddAsync(ContactMessage entity) =>
            await CreateAsync(entity);

        public async Task EditAsync(Expression<Func<ContactMessage, bool>> expression, 
            ContactMessage entity) =>
            await UpdateAsync(expression, entity);

        public async Task EditManyAsync(List<ContactMessage> entities)
        {
            foreach (var entity in entities)
            {
                await UpdateAsync(x => x.Id.Equals(entity.Id), entity);
            }
        }

        public async Task DeleteAsync(Expression<Func<ContactMessage, bool>> expression) =>
            await RemoveAsync(expression);

        public IQueryable<ContactMessage> FindAsQueryable(Expression<Func<ContactMessage, bool>> expression)
            => GetAsQueryable(expression);

        public async Task<ContactMessage?> FindByConditionAsync(Expression<Func<ContactMessage, bool>> expression)
            => await GetAsync(expression);

        public async Task<bool> RecordExistsAsync(Expression<Func<ContactMessage, bool>> expression)
            => await ExistsAsync(expression);

        public async Task<long> RecordsCountAsync(Expression<Func<ContactMessage, bool>> expression)
            => await CountAsync(expression);
    }
}
