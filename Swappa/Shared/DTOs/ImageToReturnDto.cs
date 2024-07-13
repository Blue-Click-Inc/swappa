namespace Swappa.Shared.DTOs
{
    public class ImageToReturnDto
    {
        public Guid Id { get; set; }
        public string Url { get; set; } = string.Empty;
        public string PublicId { get; set; } = string.Empty;
        public bool IsMain { get; set; }
    }
}
