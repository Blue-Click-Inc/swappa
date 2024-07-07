using Swappa.Entities.Enums;

namespace Swappa.Data.Services.Interfaces
{
    public interface ICommon
    {
        string GetLoggedInUserId();
        Task<List<SystemRole>> GetUserRoles();
        Task<bool> HasEqualOrHigherRole(SystemRole role);
    }
}
