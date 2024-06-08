using Swappa.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace Swappa.Shared.DTOs
{
    public record UserDetailsForUpdateDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        public Gender Gender { get; set; }
    }
}
