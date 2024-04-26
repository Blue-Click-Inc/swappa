using Swappa.Shared.DTOs.Interfaces;

namespace Swappa.Shared.DTOs
{
    public record RoleDto : BaseRoleDto, IBaseIdDto
    {
        public Guid Id { get; set; }
    }
}
