namespace Swappa.Shared.DTOs
{
    public record ForgotPasswordDto : ChangePasswordDto
    {
        public TokenDto? Token { get; set; }
    }
}
