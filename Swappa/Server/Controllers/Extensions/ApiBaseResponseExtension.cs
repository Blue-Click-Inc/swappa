using Swappa.Entities.Responses;

namespace Swappa.Server.Controllers.Extensions
{
    public static class ApiBaseResponseExtension
    {
        public static TResultType GetResult<TResultType>(this ApiBaseResponse apiBaseResponse) =>
            ((ApiOkResponse<TResultType>)apiBaseResponse).Result;
    }
}
