using MediatR;
using Swappa.Shared.DTOs;

namespace Swappa.Server.Queries.Location
{
    public class GetCountriesQuery : PageDto, IRequest<ResponseModel<List<CountryData>>>
    {
    }
}
