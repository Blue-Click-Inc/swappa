using Swappa.Shared.DTOs;

namespace Swappa.Client.Pages.Admin
{
    public partial class ContactMessage
    {
        private bool _isLoading = false;
        private bool _hasError = false;
        public PageDto Query { get; set; } = new();
        public HashSet<Guid> SelectedValues { get; set; } = new();
        public PaginatedListDto<ContactMessageToReturnDto>? Messages { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await GetDataAsync();
            await base.OnInitializedAsync();
        }

        private async Task GetDataAsync()
        {
            _isLoading = Messages == null;
            var query = SharedService.GetQuery(Query);
            var response = await ContactMessageService.GetDataAsync(query);
            if (response is { IsSuccessful: true })
            {
                Messages = response.Data;
            }
            else
            {
                _hasError = true;
            }
            _isLoading = false;
        }

        private async Task OnPageChangedAsync(int newPageNumber)
        {
            Query.PageNumber = newPageNumber;
            await GetDataAsync();
        }

        private async Task Clear()
        {
            Query = new();
            await GetDataAsync();
        }

        private async Task Search()
        {
            await GetDataAsync();
        }
    }
}
