using Microsoft.AspNetCore.Components;
using Swappa.Shared.DTOs;

namespace Swappa.Client.Pages.Components
{
    public partial class BaseSearch
    {
        [Parameter]
        public PageDto Query { get; set; } = new();
        [Parameter]
        public EventCallback ClearSearch { get; set; }

        protected async override Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
        }
    }
}
