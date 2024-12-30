using Swappa.Shared.DTOs;

namespace Swappa.Client.Services.Interfaces
{
    public interface IContactMessageService
    {
        Task<ResponseModel<string>?> AddAsync(ContactMessageToAddDto request);
        Task<ResponseModel<string>?> DeprecateManyAsync(List<Guid> ids);
        Task<ResponseModel<ContactMessageToReturnDto>?> GetAsync(Guid id);
        Task<ResponseModel<PaginatedListDto<ContactMessageToReturnDto>>?> GetDataAsync(string query);
        Task<ResponseModel<bool>?> ToggleIsRead(Guid id);
    }
}
