using Microsoft.AspNetCore.Components;

namespace Swappa.Client.Pages.Components
{
    public partial class KeyValueMutedDisplay
    {
        [Parameter]
        public string Key { get; set; } = string.Empty;
        [Parameter]
        public string Value { get; set; } = string.Empty;
    }
}
