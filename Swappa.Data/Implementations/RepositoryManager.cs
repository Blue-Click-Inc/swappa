using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Mongo.Common.Settings;
using Swappa.Data.Configurations;
using Swappa.Data.Contracts;

namespace Swappa.Data.Implementations
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly Lazy<ITokenRepository> _tokenRepository;

        public RepositoryManager(IOptions<MongoDbSettings> mongoSetting, 
            IOptions<CloudinarySettings> cloudSetting, IConfiguration configuration)
        {
            _tokenRepository = new Lazy<ITokenRepository>(() =>
                new TokenRepository(mongoSetting));
        }

        public ITokenRepository Token => _tokenRepository.Value;
    }
}
