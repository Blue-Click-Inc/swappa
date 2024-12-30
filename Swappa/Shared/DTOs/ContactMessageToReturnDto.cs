namespace Swappa.Shared.DTOs
{
    public class ContactMessageToReturnDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string Message { get; set; } = string.Empty;
        public DateTime DateAdded { get; set; }
        public bool IsRead { get; set; }
    }
}
