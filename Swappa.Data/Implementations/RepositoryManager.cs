﻿using Microsoft.AspNetCore.Http;
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
        private readonly Lazy<IUserRepository> _userRepository;
        private readonly Lazy<IUserFeedbackRepository> _feedbackRepository;

        public RepositoryManager(IOptions<MongoDbSettings> mongoSetting, 
            IOptions<CloudinarySettings> cloudSetting, IConfiguration configuration,
            IHttpContextAccessor contextAccessor)
        {
            _tokenRepository = new Lazy<ITokenRepository>(() =>
                new TokenRepository(mongoSetting));
            _userRepository = new Lazy<IUserRepository>(() =>
                new UserRepository(contextAccessor));
            _feedbackRepository = new Lazy<IUserFeedbackRepository>(() =>
                new UserFeedbackRepository(mongoSetting));
        }

        public ITokenRepository Token => _tokenRepository.Value;
        public IUserRepository User => _userRepository.Value;
        public IUserFeedbackRepository Feedback => _feedbackRepository.Value;
    }
}
