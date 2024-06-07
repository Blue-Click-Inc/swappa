using Swappa.Entities.Enums;

namespace Swappa.Shared.DTOs
{
    public class UserDetailsDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public Gender Gender { get; set; }
        public string Roles { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; }
        public DateTime LastLogin { get; set; }
        public Status Status { get; set; }
        public DateTime UpdatedOn { get; set; }
        public DateTime DeactivatedOn { get; set; }
        public bool IsDeprecated { get; set; }
    }
}
