using Swappa.Shared.DTOs;

namespace Swappa.Client.Shared
{
    public partial class Faq
    {
        private bool _isLoading = false;
        private bool _hasError = false;
        public PageDto Query { get; set; } = new();
        public PaginatedListDto<FaqToReturnDto>? Faqs { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await GetDataAsync();
            await base.OnInitializedAsync();
        }

        private async Task GetDataAsync()
        {
            _isLoading = Faqs == null;
            var response = await FaqService.GetDataAsync(Query);
            if (response is { IsSuccessful: true })
            {
                Faqs = response.Data;
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
