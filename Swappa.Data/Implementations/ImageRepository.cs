using Microsoft.Extensions.Options;
using Mongo.Common.MongoDB;
using Mongo.Common.Settings;
using Swappa.Data.Contracts;
using Swappa.Entities.Models;
using System.Linq.Expressions;

namespace Swappa.Data.Implementations
{
    public class ImageRepository : Repository<Image>, IImageRepository
    {
        public ImageRepository(IOptions<MongoDbSettings> options) 
            : base(options)
        {}

        public IQueryable<Image> FindAsQueryable(Expression<Func<Image, bool>> expression) =>
            GetAsQueryable(expression);

        public async Task AddAsync(Image image) =>
            await CreateAsync(image);

        public async Task AddAsync(List<Image> images) =>
            await CreateManyAsync(images);

        public async Task AddAsync(ICollection<Image> images) =>
            await CreateManyAsync(images);

        public async Task EditAsync(Expression<Func<Image, bool>> expression, Image image) =>
            await UpdateAsync(expression, image);

        public async Task DeleteAsync(Expression<Func<Image, bool>> expression) =>
            await RemoveAsync(expression);

        public async Task<Image?> FindAsync(Expression<Func<Image, bool>> expression) =>
            await GetAsync(expression);

        public async Task<List<Image>> FindManyAsync(Expression<Func<Image, bool>> expression) =>
            await GetManyAsync(expression);

        public IQueryable<Image> FindAsQueryable() =>
            GetAsQueryable();

        public async Task<long> Count(Expression<Func<Image, bool>> expression) =>
            await CountAsync(expression);

        public async Task<bool> Exists(Expression<Func<Image, bool>> expression) =>
            await ExistsAsync(expression);
    }
}
