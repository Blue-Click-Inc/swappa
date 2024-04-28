namespace Swappa.Shared.DTOs
{
    public abstract record EmailDto
    {
        public string Email { get; init; } = string.Empty;
    }
}
