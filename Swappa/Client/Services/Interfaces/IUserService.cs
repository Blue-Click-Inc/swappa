using Swappa.Shared.DTOs;

namespace Swappa.Client.Services.Interfaces
{
    public interface IUserService
    {
        Task<ResponseModel<UserDetailsDto>?> GetUserByIdAsync(Guid id);
        Task<Guid> GetLoggedInUserId();
        Task<ResponseModel<string>?> UpdateDetailsAsync(Guid id, UserDetailsForUpdateDto request);
        Task<ResponseModel<string>?> SendFeedbackAsync(FeedbackForAddDto request);
        Task<ResponseModel<PaginatedListDto<UserFeedbackDto>>?> GetUsersFeedbacks(PageDto request);
        Task<ResponseModel<FeedbackDashboardDto>?> FeedbackDashboardData();
        Task<ResponseModel<string>?> ToggleFeedbackAsync(Guid id);
        Task<ResponseModel<string>?> DeleteFeedbackAsync(Guid id);
        Task<ResponseModel<UserDashboardDataDto>?> GetUserDashboardAsync();
        Task<ResponseModel<PaginatedListDto<LeanUserDetailsDto>>?> GetPagedUsersAsync(PageDto request);
    }
}
