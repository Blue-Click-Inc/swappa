namespace Swappa.Shared.DTOs
{
    public class LocationToReturnDto
    {
        public Guid Id { get; set; }
        public string Country { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
    }
}
