namespace Swappa.Shared.DTOs
{
    public abstract record BaseRoleDto
    {
        public string RoleName { get; init; } = string.Empty;
    }
}
