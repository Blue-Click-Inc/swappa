using Microsoft.Extensions.Primitives;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Swappa.Shared.DTOs
{
    public record ForgotPasswordDto
    {
        [JsonIgnore]
        public StringValues Origin { get; set; }
        public string Token { get; set; } = string.Empty;
        [Required(ErrorMessage = "New Password is required.")]
        public string NewPassword { get; set; } = string.Empty;
        [Required(ErrorMessage = "Confirm New Password is required.")]
        [Compare(nameof(NewPassword), ErrorMessage = "New Password and Confirm New Password must match.")]
        public string ConfirmNewPassword { get; set; } = string.Empty;
    }
}
