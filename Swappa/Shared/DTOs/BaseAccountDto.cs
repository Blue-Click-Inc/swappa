using System.ComponentModel.DataAnnotations;

namespace Swappa.Shared.DTOs
{
    public abstract record BaseAccountDto
    {
        [EmailAddress, Required]
        public string Email { get; init; } = string.Empty;
        [Required]
        public string Password { get; init; } = string.Empty;
    }
}
