using Newtonsoft.Json;

namespace Swappa.Shared.DTOs
{
    public class StateDataToReturnDto : StateDataBaseDto
    {
        [JsonIgnore]
        public string _Id { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
