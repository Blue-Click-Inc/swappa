using Swappa.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace Swappa.Shared.DTOs
{
    public sealed record RegisterDto : BaseAccountDto
    {
        [Required]
        public string Name { get; init; } = string.Empty;
        [Compare("Password")]
        public string ConfirmPassword { get; init; } = string.Empty;
        public Gender Gender { get; init; } = Gender.Others;
    }
}
