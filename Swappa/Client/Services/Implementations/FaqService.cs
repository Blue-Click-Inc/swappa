using Swappa.Client.Services.Interfaces;
using Swappa.Entities.Enums;
using Swappa.Shared.DTOs;
using Swappa.Shared.Extensions;
using System.Net.Http.Json;

namespace Swappa.Client.Services.Implementations
{
    public class FaqService : IFaqService
    {
        private readonly HttpClient httpClient;
        private readonly HttpInterceptorService httpInterceptor;

        public FaqService(HttpClient httpClient,
            HttpInterceptorService httpInterceptor)
        {
            this.httpClient = httpClient;
            this.httpInterceptor = httpInterceptor;
        }

        public async Task<ResponseModel<PaginatedListDto<FaqToReturnDto>>?> GetDataAsync(PageDto query)
        {
            var response = await httpClient.GetAsync($"faq{GetQuery(query)}");
            return await httpInterceptor.Process<ResponseModel<PaginatedListDto<FaqToReturnDto>>>(response);
        }

        public async Task<ResponseModel<string>?> AddAsync(FaqToCreateDto request)
        {
            var response = await httpClient.PostAsJsonAsync($"faq", request);
            return await httpInterceptor.Process<ResponseModel<string>>(response);
        }

        public async Task<ResponseModel<string>?> UpdateAsync(Guid id, FaqToUpdateDto request)
        {
            var response = await httpClient.PutAsJsonAsync($"faq/{id}", request);
            return await httpInterceptor.Process<ResponseModel<string>>(response);
        }

        public async Task<ResponseModel<string>?> DeleteAsync(Guid id)
        {
            var response = await httpClient.DeleteAsync($"faq/{id}");
            return await httpInterceptor.Process<ResponseModel<string>>(response);
        }

        public async Task<ResponseModel<FaqToReturnDto>?> GetAsync(Guid id)
        {
            var response = await httpClient.GetAsync($"faq/{id}");
            return await httpInterceptor.Process<ResponseModel<FaqToReturnDto>>(response);
        }

        private string GetQuery(PageDto query)
        {
            var queryStr = string.Empty;
            if (query.SearchTerm.IsNotNullOrEmpty())
            {
                queryStr += queryStr.IsNotNullOrEmpty() ? $"&SearchTerm={query.SearchTerm}" : $"?SearchTerm={query.SearchTerm}";
            }
            if (query.PageSize != default)
            {
                queryStr += queryStr.IsNotNullOrEmpty() ? $"&PageSize={query.PageSize}" : $"?PageSize={query.PageSize}";
            }
            if (query.PageNumber != default)
            {
                queryStr += queryStr.IsNotNullOrEmpty() ? $"&PageNumber={query.PageNumber}" : $"?PageNumber={query.PageNumber}";
            }

            return queryStr;
        }
    }
}
