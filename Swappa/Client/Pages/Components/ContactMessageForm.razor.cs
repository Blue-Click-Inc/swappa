using Swappa.Shared.DTOs;

namespace Swappa.Client.Pages.Components
{
    public partial class ContactMessageForm
    {
        private bool _isLoading = false;
        
        public ContactMessageToAddDto Message { get; set; } = new();

        async Task SendAsync()
        {
            _isLoading = true;
            var response = await ContactMessageService.AddAsync(Message);
            if (response != null && response.IsSuccessful)
            {
                Message = new();
                Toast.ShowSuccess(response.Data ?? string.Empty);
            }
            else
            {
                Toast.ShowError(response?.Message ?? string.Empty);
            }
            _isLoading = false;
        }
    }
}