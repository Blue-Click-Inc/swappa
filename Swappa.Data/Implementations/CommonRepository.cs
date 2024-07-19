using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Swappa.Data.Contracts;
using Swappa.Entities.Enums;
using Swappa.Entities.Models;
using Swappa.Shared.Extensions;
using System.Security.Claims;

namespace Swappa.Data.Implementations
{
    internal class CommonRepository : ICommonRepository
    {
        private readonly IHttpContextAccessor contextAccessor;
        private readonly UserManager<AppUser> userManager;

        public CommonRepository(IHttpContextAccessor contextAccessor,
            UserManager<AppUser> userManager)
        {
            this.contextAccessor = contextAccessor;
            this.userManager = userManager;
        }

        public string GetLoggedInUserId() => GetUserId();

        public async Task<List<SystemRole>> GetUserRoles()
        {
            var userId = GetUserId();
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new List<SystemRole>();
            }

            return (await userManager.GetRolesAsync(user)).ToList().ParseValues<SystemRole>();
        }

        public async Task<bool> HasEqualOrHigherRole(SystemRole role)
        {
            var roles = await GetUserRoles();
            return roles.Any(x => (int)x >= (int)role);
        }

        private string GetUserId()
        {
            ClaimsPrincipal? userClaim = contextAccessor.HttpContext?.User;
            return userClaim?.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
        }
    }
}
