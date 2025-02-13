using Refit;
using Swappa.Shared.DTOs;

namespace Swappa.Shared.Interface
{
    public interface IExternalLocation
    {
        /// <summary>
        /// Gets country by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A country object</returns>
        [Get("/country/{id}")]
        Task<CountryData> GetCountry(Guid id);

        /// <summary>
        /// Gets a list of countries
        /// </summary>
        /// <returns>Paginated list of countries</returns>
        [Get("/countries")]
        Task<CountryPaged> GetCountriesAsync();

        /// <summary>
        /// Gets a list of states in a country
        /// </summary>
        /// <param name="countryId"></param>
        /// <returns>A list of state objects</returns>
        [Get("/country/{countryId}/states")]
        Task<List<StateData>> GetStatesAsync(Guid countryId);

        /// <summary>
        /// Gets a list of cities in a state
        /// </summary>
        /// <param name="countryId"></param>
        /// <param name="stateId"></param>
        /// <returns>List of cities objects</returns>
        [Get("/country/{countryId}/state/{stateId}/cities")]
        Task<List<CityData>> GetCitiesAsync(Guid countryId, Guid stateId);

        /// <summary>
        /// Gets a state by id
        /// </summary>
        /// <param name="countryId"></param>
        /// <param name="id"></param>
        /// <returns>State object</returns>
        [Get("/country/{countryId}/state/{id}")]
        Task<StateData> GetState(Guid countryId, Guid id);
    }
}
