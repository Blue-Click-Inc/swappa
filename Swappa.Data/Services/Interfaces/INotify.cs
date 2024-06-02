using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Swappa.Entities.Enums;
using Swappa.Entities.Models;
using Swappa.Shared.DTOs;

namespace Swappa.Data.Services.Interfaces
{
    public interface INotify
    {
        Task<bool?> SendAsync(string to, string message, string subject, IFormFile file);
        Task<bool?> SendAsync(string to, string message, string subject);
        Task<ResponseModel<string>> SendAccountEmailAsync(AppUser user, StringValues origin, TokenType tokenType);
    }
}
