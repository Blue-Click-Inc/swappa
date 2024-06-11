namespace Swappa.Shared.DTOs
{
    public class UserFeedbacksDto
    {
        private readonly string _email;
        private readonly string _name;
        private readonly DateTime _latest;
        private readonly List<UserFeedbackDto> _feedbacks;
        public UserFeedbacksDto(string email, List<UserFeedbackDto> feedbacks)
        {
            _email = email;
            _feedbacks = feedbacks.OrderByDescending(f => f.CreatedAt).ToList();
            _name = _feedbacks.FirstOrDefault()?.UserName ?? string.Empty;
            _latest = _feedbacks
                .FirstOrDefault()?.CreatedAt ?? DateTime.MinValue;
        }

        public string Email => _email;
        public string Name => _name;
        public DateTime Latest => _latest;
        public List<UserFeedbackDto> Feedbacks => _feedbacks;
    }
}
