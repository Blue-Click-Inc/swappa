using Swappa.Shared.DTOs;

namespace Swappa.Client.Services.Interfaces
{
    public interface IContactMessageService
    {
        Task<ResponseModel<string>?> AddAsync(ContactMessageToAddDto request);
        Task<ResponseModel<FaqToReturnDto>?> GetAsync(Guid id);
        Task<ResponseModel<PaginatedListDto<ContactMessageToReturnDto>>?> GetDataAsync(string query);
        Task<ResponseModel<bool>?> ToggleIsRead(Guid id);
    }
}
