using Swappa.Entities.Enums;

namespace Swappa.Shared.DTOs
{
    public record CustomUserClaims(string Id = null!, string UserName = null!, List<string> Roles = null!);
}
