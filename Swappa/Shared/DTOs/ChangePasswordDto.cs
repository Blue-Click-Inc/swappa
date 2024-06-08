using System.ComponentModel.DataAnnotations;

namespace Swappa.Shared.DTOs
{
    public record ChangePasswordDto
    {
        [Required(ErrorMessage = "Current Password field is required")]
        public string CurrentPassword { get; set; } = string.Empty;
        [Required(ErrorMessage = "New Password field is required.")]
        public string NewPassword { get; set; } = string.Empty;
    }
}
