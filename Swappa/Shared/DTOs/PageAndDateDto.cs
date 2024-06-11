using Swappa.Shared.DTOs.Interfaces;
using Swappa.Shared.Extensions;

namespace Swappa.Shared.DTOs
{
    public class PageAndDateDto : BasePageDto, IDateRangeDto
    {
        public DateTime StartDate { get; set; } = DateExtensions.MinimumDate();
        public DateTime EndDate { get; set; } = DateExtensions.MaximumDate();
    }
}
