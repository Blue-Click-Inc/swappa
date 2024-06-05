using System.Security.Claims;

namespace Swappa.Client.Pages.Account
{
    public partial class Profile
    {
        public string? UserId { get; set; }
        protected override async Task OnInitializedAsync()
        {
            UserId = await GetUserId();
            await base.OnInitializedAsync();
        }

        private async Task<string?> GetUserId()
        {
            var state = await AuthStateProvider.GetAuthenticationStateAsync();
            return state?.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        }
    }
}
