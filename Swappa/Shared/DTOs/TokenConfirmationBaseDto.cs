namespace Swappa.Shared.DTOs
{
    public abstract record TokenConfirmationBaseDto
    {
        public string Token { get; init; } = string.Empty;
    }
}
