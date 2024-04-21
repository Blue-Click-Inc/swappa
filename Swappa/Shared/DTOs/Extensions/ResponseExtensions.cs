using Swappa.Entities.Responses;

namespace Swappa.Shared.DTOs.Extensions
{
    public static class ResponseExtensions
    {
        public static TResultType GetResult<TResultType>(this ApiBaseResponse apiBaseResponse) =>
            ((ApiOkResponse<TResultType>)apiBaseResponse).Result;
    }
}
