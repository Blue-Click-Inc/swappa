using Swappa.Entities.Enums;

namespace Swappa.Data.Contracts
{
    public interface ICommonRepository
    {
        string GetLoggedInUserId();
        Guid GetUserIdAsGuid();
        Task<List<SystemRole>> GetUserRoles();
        Task<bool> HasEqualOrHigherRole(SystemRole role);
    }
}
