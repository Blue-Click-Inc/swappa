using System.ComponentModel.DataAnnotations;

namespace Swappa.Shared.DTOs
{
    public abstract record BaseAccountDto : EmailDto
    {
        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
