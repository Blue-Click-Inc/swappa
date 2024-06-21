using Refit;
using Swappa.Shared.DTOs;

namespace Swappa.Data.Services.Interfaces
{
    [Headers("Authorization: Bearer")]
    public interface IStateService
    {
        [Get("/states?countryId={countryId}")]
        Task<List<StateDataToReturnDto>> Get(string countryId);
        [Get("/states/{id}")]
        Task<StateDataToReturnDto> GetOne(string id);
        [Post("/states/{countryId}")]
        Task Post(string countryId, [Body] StateDto state);
        [Put("/states/{id}")]
        Task Put(string id, [Body] StateDto state);
        [Delete("/states/{id}")]
        Task Delete(string id);
    }
}
