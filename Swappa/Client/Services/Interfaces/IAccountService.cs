using Swappa.Shared.DTOs;

namespace Swappa.Client.Services.Interfaces
{
    public interface IAccountService
    {
        Task<ResponseModel<TokenDto>?> LoginAsync(LoginDto login);
        Task<ResponseModel<string>?> RegisterAsync(RegisterDto register);
    }
}
