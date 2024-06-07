using Swappa.Client.Services.Implementations;
using Swappa.Client.Services.Interfaces;

namespace Swappa.Client.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureServices(this IServiceCollection services)
        {
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IUserService, UserService>();
        }
    }
}
