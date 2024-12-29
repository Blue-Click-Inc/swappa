using Swappa.Shared.DTOs;

namespace Swappa.Client.Services.Interfaces
{
    public interface IFaqService
    {
        Task<ResponseModel<string>?> AddAsync(FaqToCreateDto request);
        Task<ResponseModel<string>?> DeleteAsync(Guid id);
        Task<ResponseModel<FaqToReturnDto>?> GetAsync(Guid id);
        Task<ResponseModel<PaginatedListDto<FaqToReturnDto>>?> GetDataAsync(PageDto query);
        Task<ResponseModel<string>?> UpdateAsync(Guid id, FaqToUpdateDto request);
    }
}
