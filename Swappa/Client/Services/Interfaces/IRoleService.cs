using Swappa.Shared.DTOs;

namespace Swappa.Client.Services.Interfaces
{
    public interface IRoleService
    {
        Task<ResponseModel<string>?> AddAsync(RoleForCreateDto request);
        Task<ResponseModel<string>?> DeleteAsync(Guid id);
        Task<ResponseModel<RoleDto>?> GetAsync(Guid id);
        Task<ResponseModel<PaginatedListDto<RoleDto>>?> GetAsync(int page = 1, int pageSize = 100);
        Task<ResponseModel<string>?> UpdateAsync(Guid id, RoleForUpdateDto request);
    }
}
