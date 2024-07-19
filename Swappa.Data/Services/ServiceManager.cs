using Mailjet.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Swappa.Data.Contracts;
using Swappa.Data.Services.Interfaces;
using Swappa.Entities.Models;
using Swappa.Shared.DTOs;

namespace Swappa.Data.Services
{
    public class ServiceManager : IServiceManager
    {
        private readonly Lazy<ICloudinaryService> _cloudinaryService;
        private readonly Lazy<IMedia> _media;
        private readonly Lazy<INotify> _notify;
        private readonly Lazy<IToolService> _toolService;
        private readonly Lazy<IExportService> _exportService;

        public ServiceManager(IConfiguration configuration, 
            IRepositoryManager repository, IHttpContextAccessor contextAccessor,
            UserManager<AppUser> userManager, IMailjetClient mailjetClient,
            ApiResponseDto response)
        {
            _cloudinaryService = new Lazy<ICloudinaryService>(() =>
                new CloudinaryService(configuration, repository));
            _media = new Lazy<IMedia>(() =>
                new Media());
            _notify = new Lazy<INotify>(() =>
               new Notify(mailjetClient, configuration, userManager, repository, response));
            _toolService = new Lazy<IToolService>(() =>
                new ToolService(repository));
            _exportService = new Lazy<IExportService>(() =>
                new ExportService(repository));
        }

        public ICloudinaryService Cloudinary => _cloudinaryService.Value;
        public IMedia Media => _media.Value;
        public INotify Notify => _notify.Value;
        public IToolService Tool => _toolService.Value;
        public IExportService Export => _exportService.Value;
    }
}
