using Swappa.Shared.DTOs.Interfaces;
using Swappa.Shared.Extensions;
using System.Text.Json.Serialization;

namespace Swappa.Shared.DTOs
{
    public class DateRangeDto : IDateRangeDto
    {
        public DateTime StartDate { get; set; } = DateExtensions.MinimumDate();
        public DateTime EndDate { get; set; } = DateExtensions.MaximumDate();
        [JsonIgnore]
        public bool IsValid => StartDate <= EndDate;
    }
}
