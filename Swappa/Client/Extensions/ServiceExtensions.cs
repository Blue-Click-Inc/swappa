using Swappa.Client.Services.Implementations;
using Swappa.Client.Services.Interfaces;
using Toolbelt.Blazor.Extensions.DependencyInjection;

namespace Swappa.Client.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureServices(this IServiceCollection services)
        {
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ISharedService, SharedService>();
            services.AddScoped<HttpInterceptorService>();
            services.AddScoped<ILocationService, LocationService>();
            services.AddScoped<IToolsService, ToolsService>();
            services.AddScoped<IVehicleService, VehicleService>();
            services.AddScoped<IFaqService, FaqService>();
        }

        public static void ConfigureInterceptor(this IServiceCollection services)
        {
            var provider = services.BuildServiceProvider();
            var client = provider.GetRequiredService<HttpClient>();
            client.EnableIntercept(provider);
            services.AddHttpClientInterceptor();
        }
    }
}
