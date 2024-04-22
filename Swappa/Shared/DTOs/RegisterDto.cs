using Microsoft.Extensions.Primitives;
using Swappa.Entities.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Swappa.Shared.DTOs
{
    public record RegisterDto : BaseAccountDto
    {
        [Required]
        public string Name { get; init; } = string.Empty;
        [Compare("Password")]
        public string ConfirmPassword { get; init; } = string.Empty;
        public Gender Gender { get; init; } = Gender.NotSpecified;
        public Role Role { get; set; } = Role.User;
        [JsonIgnore]
        public StringValues Origin { get; set; }
    }
}
