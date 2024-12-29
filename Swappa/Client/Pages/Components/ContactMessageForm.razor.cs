using Blazored.Modal;
using Microsoft.AspNetCore.Components;
using Swappa.Shared.DTOs;

namespace Swappa.Client.Pages.Components
{
    public partial class ContactMessageForm
    {
        ContactMessage Message { get; set; } = new();
        async Task SendAsync()
        {
            await Task.Run(() => Console.WriteLine());
        }
    }
}
