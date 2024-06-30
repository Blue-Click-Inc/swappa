using Swappa.Shared.DTOs.Interfaces;

namespace Swappa.Shared.DTOs
{
    public class RoleDto : BaseRoleDto, IBaseIdDto
    {
        public Guid Id { get; set; }
        public long NumberOfUser { get; set; }
    }
}
