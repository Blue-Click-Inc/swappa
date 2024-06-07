using Swappa.Shared.DTOs;

namespace Swappa.Client.Services.Interfaces
{
    public interface IUserService
    {
        Task<ResponseModel<UserDetailsDto>?> GetUserByIdAsync(Guid id);
        Task<Guid> GetLoggedInUserId();
    }
}
