using Microsoft.AspNetCore.Http;

namespace Swappa.Shared.DTOs
{
    public class FileDto
    {
        public IFormFile? File { get; set; }
    }
}
