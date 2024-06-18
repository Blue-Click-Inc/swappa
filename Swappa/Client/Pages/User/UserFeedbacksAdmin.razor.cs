using Swappa.Shared.DTOs;

namespace Swappa.Client.Pages.User
{
    public partial class UserFeedbacksAdmin
    {
        public PageAndDateDto Query { get; set; } = new();
        public ResponseModel<PaginatedListDto<UserFeedbackDto>>? Data { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await GetFeedbacks();
            
            await base.OnInitializedAsync();
        }

        private async Task GetFeedbacks()
        {
            var feedbacks = await UserService.GetUsersFeedbacks(Query);
            Data = feedbacks;
        }
    }
}