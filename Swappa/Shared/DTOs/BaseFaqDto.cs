using System.ComponentModel.DataAnnotations;

namespace Swappa.Shared.DTOs
{
    public abstract class BaseFaqDto
    {
        [Required(ErrorMessage = "The Title field is required")]
        public string Title { get; set; } = string.Empty;
        [Required(ErrorMessage = "The Details field is required")]
        public string Details { get; set; } = string.Empty;
    }
}
