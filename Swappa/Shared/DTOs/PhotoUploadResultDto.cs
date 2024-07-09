namespace Swappa.Shared.DTOs
{
    public record PhotoUploadResultDto
    {
        public string Url { get; init; } = string.Empty;
        public string PublicId { get; init; } = string.Empty;
    }
}
