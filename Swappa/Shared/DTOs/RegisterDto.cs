using Microsoft.Extensions.Primitives;
using Swappa.Entities.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Swappa.Shared.DTOs
{
    public record RegisterDto : BaseAccountDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        [Compare("Password", ErrorMessage = "Password and Compare Password fields must match")]
        public string ConfirmPassword { get; set; } = string.Empty;
        public Gender Gender { get; set; } = Gender.NotSpecified;
        public Role Role { get; set; } = Role.User;
        [JsonIgnore]
        public StringValues Origin { get; set; }
        [JsonIgnore]
        public bool MatchPassword => Password.Equals(ConfirmPassword);
    }
}
