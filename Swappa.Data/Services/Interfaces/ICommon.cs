using Swappa.Entities.Enums;

namespace Swappa.Data.Services.Interfaces
{
    public interface ICommon
    {
        string GetLoggedInUserId();
        Task<IList<SystemRole>> GetUserRoles();
        Task<bool> HasEqualOrHigherRole(SystemRole role);
    }
}
