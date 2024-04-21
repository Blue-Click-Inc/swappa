using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Mongo.Common.Settings;
using Swappa.Data.Configurations;
using Swappa.Data.Contracts;

namespace Swappa.Data.Implementations
{
    public class RepositoryManager : IRepositoryManager
    {
        public RepositoryManager(IOptions<MongoDbSettings> mongoSetting, 
            IOptions<CloudinarySettings> cloudSetting, IConfiguration configuration)
        {

        }
    }
}
