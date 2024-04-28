namespace Swappa.Shared.DTOs
{
    public record ChangePasswordDto : BaseAccountDto
    {
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
