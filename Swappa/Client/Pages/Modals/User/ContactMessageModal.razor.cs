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

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
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
