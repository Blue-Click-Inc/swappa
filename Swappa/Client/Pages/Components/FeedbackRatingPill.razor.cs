using Microsoft.AspNetCore.Components;
using Swappa.Entities.Enums;

namespace Swappa.Client.Pages.Components
{
    public partial class FeedbackRatingPill
    {
        private string pillClass = string.Empty;
        [Parameter]
        public FeedbackRating Background { get; set; }
        [Parameter]
        public string PillContent { get; set; } = string.Empty;

        protected override async Task OnInitializedAsync()
        {
            SetClass();
            await base.OnInitializedAsync();
        }

        private void SetClass()
        {
            switch (Background)
            {
                case FeedbackRating.None:
                    pillClass = "secondary";
                    break;
                case FeedbackRating.VeryBad:
                case FeedbackRating.Bad:
                    pillClass = "danger";
                    break;
                case FeedbackRating.Average:
                    pillClass = "warning";
                    break;
                case FeedbackRating.Good:
                    pillClass = "primary";
                    break;
                case FeedbackRating.Excellent:
                    pillClass = "success";
                    break;
            }
        }
    }
}
