namespace Swappa.Shared.DTOs
{
    public record TokenDto
    {
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
        public TokenDto(string accessToken, string refreshToken)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
        }
    }
}
