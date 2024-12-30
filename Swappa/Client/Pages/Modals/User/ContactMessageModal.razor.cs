using Blazored.Modal;
using Microsoft.AspNetCore.Components;
using Swappa.Shared.DTOs;
using Swappa.Shared.Extensions;

namespace Swappa.Client.Pages.Modals.User
{
    public partial class ContactMessageModal
    {
        private bool _isLoading = false;
        private bool _hasError = false;

        [Parameter]
        public Guid Id { get; set; }
        [CascadingParameter]
        BlazoredModalInstance Instance { get; set; } = new();
        public ContactMessageToReturnDto? Data { get; set; }
        public ResponseMessageDto Reply { get; set; } = new();

        protected override async Task OnParametersSetAsync()
        {
            await GetDataByIdAsync();
            await base.OnParametersSetAsync();
        }

        public async Task ReplyAsync()
        {
            _isLoading = true;
            Reply.Name = Data.Name;
            Reply.Email = Data.Email;
            var response = await ContactMessageService.SendReply(Reply);
            if(response.IsNotNull() && response.IsSuccessful)
            {
                Toast.ShowSuccess(response?.Data ?? "Operation successful");
                await Instance.CloseAsync();
            }
            else
            {
                _hasError = true;
                Toast.ShowError(response?.Message ?? "An error occurred. Please try again later.");
            }
            _isLoading = false;
            await Task.Run(() => true);
        }

        public async Task GetDataByIdAsync()
        {
            if (Id.IsNotEmpty())
            {
                _isLoading = true;
                var response = await ContactMessageService.GetAsync(Id);
                if(response.IsNotNull() && response.IsSuccessful)
                {
                    Data = response.Data;
                }
                else
                {
                    _hasError = true;
                }
            }

            _isLoading = false;
        }
    }
}
