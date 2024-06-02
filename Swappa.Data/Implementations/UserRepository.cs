using Microsoft.AspNetCore.Http;
using Swappa.Data.Contracts;
using System.Security.Claims;

namespace Swappa.Data.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly IHttpContextAccessor httpContext;

        public UserRepository(IHttpContextAccessor httpContext)
        {
            this.httpContext = httpContext;
        }

        public Guid GetLoogedInUserId()
        {
            ClaimsPrincipal? userClaim = httpContext.HttpContext?.User;
            var userId = userClaim?.FindFirstValue(ClaimTypes.NameIdentifier);
            Guid.TryParse(userId, out Guid id);

            return id;
        }
    }
}
