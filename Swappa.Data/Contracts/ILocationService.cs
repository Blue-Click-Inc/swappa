using Swappa.Entities.Models;
using Swappa.Shared.DTOs;
using System.Linq.Expressions;

namespace Swappa.Data.Contracts
{
    public interface ILocationService
    {
        Task AddAsync(CountryDataDto request);
        Task DeleteAsync(string countryId);
        Task<List<CountryDataToReturnDto>> GetAsync(int page, int pageSize);
        Task<CountryDataToReturnDto?> GetAsync(string countryId);
        Task<List<StateDataToReturnDto>> GetManyAsync(string countryId);
        Task<StateDataToReturnDto?> GetOneAsync(string id);
        Task EditAsync(string countryId, CountryDataDto request);
        Task AddAsync(EntityLocation entity);
        Task EditAsync(Expression<Func<EntityLocation, bool>> expression, EntityLocation entity);
        Task DeleteAsync(Expression<Func<EntityLocation, bool>> expression);
        Task<EntityLocation?> GetByConditionAsync(Expression<Func<EntityLocation, bool>> expression);
        Task<bool> Exists(Expression<Func<EntityLocation, bool>> expression);
    }
}
