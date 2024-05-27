using Swappa.Shared.DTOs;

namespace Swappa.Client.Services.Interfaces
{
    public interface IRoleService
    {
        Task<PaginatedListDto<RoleDto>?> GetRolesAsync(int page = 1, int pageSize = 10);
    }
}
