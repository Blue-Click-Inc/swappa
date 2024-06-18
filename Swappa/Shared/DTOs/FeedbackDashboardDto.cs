using Swappa.Entities.Enums;

namespace Swappa.Shared.DTOs
{
    public class FeedbackDashboardDto
    {
        public long TotalFeedbacks { get; set; }
        public FeedbackRating AverageRating { get; set; }
    }
}
