using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Swappa.Data.Services.Interfaces;
using Swappa.Entities.Enums;
using Swappa.Entities.Models;
using Swappa.Shared.Extensions;
using System.Security.Claims;

namespace Swappa.Data.Services
{
    public class Common : ICommon
    {
        private readonly IHttpContextAccessor contextAccessor;
        private readonly UserManager<AppUser> userManager;

        public Common(IHttpContextAccessor contextAccessor,
            UserManager<AppUser> userManager)
        {
            this.contextAccessor = contextAccessor;
            this.userManager = userManager;
        }

        public string GetLoggedInUserId()
        {
            return GetUserId();
        }

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

        /// <summary>
        /// Checks if the logged in user has a role with equal or higher rights than the specified role
        /// </summary>
        /// <param name="role">The role against which we are checking the user rights</param>
        /// <returns></returns>
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
