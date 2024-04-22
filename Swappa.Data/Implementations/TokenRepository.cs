using Microsoft.Extensions.Options;
using Mongo.Common.MongoDB;
using Mongo.Common.Settings;
using Swappa.Data.Contracts;
using Swappa.Entities.Models;
using System.Linq.Expressions;

namespace Swappa.Data.Implementations
{
    internal class TokenRepository : Repository<Token>, ITokenRepository
    {
        public TokenRepository(IOptions<MongoDbSettings> options) 
            : base(options) {}

        public async Task AddAsync(Token token) =>
            await CreateAsync(token);

        public async Task DeleteAsync(Expression<Func<Token, bool>> expression) =>
            await RemoveAsync(expression);

        public async Task<Token?> FindAsync(Expression<Func<Token, bool>> expression) =>
            await GetAsync(expression);
    }
}
