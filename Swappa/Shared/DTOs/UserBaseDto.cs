using Swappa.Entities.Enums;

namespace Swappa.Shared.DTOs
{
    public abstract class UserBaseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public Gender Gender { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime LastLogin { get; set; }
        public DateTime UpdatedOn { get; set; }
        public DateTime DeactivatedOn { get; set; }
        public Status Status { get; set; }
        public bool IsDeprecated { get; set; }
    }
}
