using Swappa.Entities.Enums;

namespace Swappa.Shared.DTOs
{
    public class UserDetailsDto : UserBaseDto
    {
        public List<SystemRole> UserRoles { get; set; } = new();
        public LocationToReturnDto? Location { get; set; }
    }
}