using Newtonsoft.Json;

namespace Swappa.Shared.DTOs
{
    public class CountryDataToReturnDto : CountryBaseDto
    {
        public string _Id { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
