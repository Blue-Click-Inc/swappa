using Microsoft.AspNetCore.Components;
using Swappa.Entities.Enums;

namespace Swappa.Client.Pages.Components
{
    public partial class SinglePillCardDash
    {
        [Parameter]
        public FeedbackRating Background { get; set; }
        [Parameter]
        public string PillContent { get; set; } = string.Empty;
        [Parameter]
        public string CardTitle { get; set; } = string.Empty;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
        }
    }
}
