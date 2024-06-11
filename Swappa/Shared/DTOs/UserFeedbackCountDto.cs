namespace Swappa.Shared.DTOs
{
    public class UserFeedbackCountDto
    {
        private readonly string _email;
        private readonly string _name;
        private readonly DateTime _latest;
        private readonly int _feedbackCount;
        public UserFeedbackCountDto(string email, List<UserFeedbackDto> feedbacks)
        {
            _email = email;
            feedbacks = feedbacks.OrderByDescending(f => f.CreatedAt).ToList();
            _name = feedbacks.FirstOrDefault()?.UserName ?? string.Empty;
            _latest = feedbacks
                .FirstOrDefault()?.CreatedAt ?? DateTime.MinValue;
            _feedbackCount = feedbacks.Count();
        }

        public string Email => _email;
        public string Name => _name;
        public DateTime Latest => _latest;
        public int FeedbackCount => _feedbackCount;
    }
}
