using Refit;
using Swappa.Shared.DTOs;

namespace Swappa.Data.Services.Interfaces
{
    [Headers("Authorization: Bearer")]
    public interface ICountryService
    {
        [Get("/countries?page={page}&pageNumber={pageSize}")]
        Task<List<CountryDataToReturnDto>> Get(int page, int pageSize);
        [Get("/countries/{countryId}")]
        Task<CountryDataToReturnDto> Get(string countryId);
        [Post("/countries")]
        Task Post([Body] CountryDataDto country);
        [Put("/countries/{countryId}")]
        Task Put(string countryId, [Body] CountryDataDto country);
        [Delete("/countries/{countryId}")]
        Task Delete(string countryId);
    }
}
