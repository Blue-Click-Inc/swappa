using Swappa.Entities.Enums;

namespace Swappa.Data.Contracts
{
    public interface ICommonRepository
    {
        string GetLoggedInUserId();
        Task<List<SystemRole>> GetUserRoles();
        Task<bool> HasEqualOrHigherRole(SystemRole role);
    }
}
