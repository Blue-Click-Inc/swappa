using Swappa.Entities.Enums;
using Swappa.Shared.DTOs;

namespace Swappa.Client.Services.Interfaces
{
    public interface IAccountService
    {
        Task<ResponseModel<string>?> AssignToRole(Guid userId, SystemRole role);
        Task<ResponseModel<string>?> ChangeForgotPasswordAsync(ForgotPasswordDto request);
        Task<ResponseModel<string>?> ChangePasswordAsync(Guid id, ChangePasswordDto request);
        Task<ResponseModel<string>?> ConfirmAccountAsync(AccountConfirmationDto account);
        Task<ResponseModel<string>?> DeactivateAccountAsync(Guid id);
        string GetUserIdFromToken(string token);
        Task<ResponseModel<TokenDto>?> LoginAsync(LoginDto login);
        Task<ResponseModel<string>?> RegisterAsync(RegisterDto register);
        Task<ResponseModel<string>?> RemoveFromRole(Guid userId, SystemRole role);
        Task<ResponseModel<string>?> ResetPasswordAsync(EmailDto request);
        Task<ResponseModel<string>?> ToggleStatus(Guid id);
    }
}
