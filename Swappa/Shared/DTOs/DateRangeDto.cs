using Swappa.Shared.DTOs.Interfaces;
using Swappa.Shared.Extensions;

namespace Swappa.Shared.DTOs
{
    public class DateRangeDto : IDateRangeDto
    {
        public DateTime StartDate { get; set; } = DateExtensions.MinimumDate();
        public DateTime EndDate { get; set; } = DateExtensions.MaximumDate();
    }
}
