using MediatR;
using Swappa.Shared.DTOs;

namespace Swappa.Server.Queries.Location
{
    public class GetCountriesQuery : BasePageDto, IRequest<ResponseModel<List<CountryDataToReturnDto>>>
    {
    }
}
