using Swappa.Shared.DTOs;

namespace Swappa.Client.Pages.Components
{
    public partial class ContactMessageForm
    {
        ContactMessageToAddDto Message { get; set; } = new();

        async Task SendAsync()
        {
            await Task.Run(() => Console.WriteLine());
        }
    }
}