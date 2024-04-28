namespace Swappa.Shared.DTOs
{
    public abstract record BaseAccountDto : EmailDto
    {
        public string Password { get; init; } = string.Empty;
    }
}
