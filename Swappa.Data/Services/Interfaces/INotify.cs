using Microsoft.AspNetCore.Http;

namespace Swappa.Data.Services.Interfaces
{
    public interface INotify
    {
        Task<bool?> SendAsync(string to, string message, string subject, IFormFile file);
        Task<bool?> SendAsync(string to, string message, string subject);
    }
}
