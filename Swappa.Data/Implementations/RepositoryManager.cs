using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Mongo.Common.Settings;
using RedisCache.Common.Repository;
using Swappa.Data.Configurations;
using Swappa.Data.Contracts;

namespace Swappa.Data.Implementations
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly Lazy<ITokenRepository> _tokenRepository;
        private readonly Lazy<IUserRepository> _userRepository;
        private readonly Lazy<IUserFeedbackRepository> _feedbackRepository;
        private readonly Lazy<ILocationService> _locationService;
        private readonly Lazy<IVehicleRepository> _vehicleRepository;
        private readonly Lazy<IImageRepository> _imageRepository;

        public RepositoryManager(IOptions<MongoDbSettings> mongoSetting, 
            IOptions<CloudinarySettings> cloudSetting, IConfiguration configuration,
            IHttpContextAccessor contextAccessor, ICacheCommonRepository redisCache)
        {
            _tokenRepository = new Lazy<ITokenRepository>(() =>
                new TokenRepository(mongoSetting));
            _userRepository = new Lazy<IUserRepository>(() =>
                new UserRepository(contextAccessor));
            _feedbackRepository = new Lazy<IUserFeedbackRepository>(() =>
                new UserFeedbackRepository(mongoSetting));
            _locationService = new Lazy<ILocationService>(() =>
                new LocationService(configuration, mongoSetting, redisCache));
            _vehicleRepository = new Lazy<IVehicleRepository>(() =>
                 new VehicleRepository(mongoSetting));
            _imageRepository = new Lazy<IImageRepository>(() =>
                new ImageRepository(mongoSetting));
        }

        public ITokenRepository Token => _tokenRepository.Value;
        public IUserRepository User => _userRepository.Value;
        public IUserFeedbackRepository Feedback => _feedbackRepository.Value;
        public ILocationService Location => _locationService.Value;
        public IVehicleRepository Vehicle => _vehicleRepository.Value;
        public IImageRepository Image => _imageRepository.Value;
    }
}
