using Swappa.Shared.DTOs;

namespace Swappa.Client.Services.Interfaces
{
    public interface IAccountService
    {
        Task<ResponseModel<string>?> ChangeForgotPasswordAsync(ForgotPasswordDto request);
        Task<ResponseModel<string>?> ConfirmAccountAsync(AccountConfirmationDto account);
        Task<ResponseModel<TokenDto>?> LoginAsync(LoginDto login);
        Task<ResponseModel<string>?> RegisterAsync(RegisterDto register);
    }
}
