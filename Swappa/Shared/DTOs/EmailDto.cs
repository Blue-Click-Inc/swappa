using System.ComponentModel.DataAnnotations;

namespace Swappa.Shared.DTOs
{
    public record EmailDto
    {
        [EmailAddress, Required]
        public string Email { get; set; } = string.Empty;
    }
}
