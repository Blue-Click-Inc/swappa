using Microsoft.AspNetCore.Http;
using Swappa.Entities.Responses;

namespace Swappa.Shared.DTOs.Extensions
{
    public static class ResponseExtensions
    {
        public static TResultType GetResult<TResultType>(this ApiBaseResponse apiBaseResponse) =>
            ((ApiOkResponse<TResultType>)apiBaseResponse).Result;

        public static ResponseModel<TResult> ProcessResponse<TResult>(this ApiBaseResponse baseResponse)
        {
            return baseResponse switch
            {
                ApiNotFoundResponse => new ResponseModel<TResult>
                {
                    Message = ((ApiNotFoundResponse)baseResponse).Message,
                    StatusCode = StatusCodes.Status404NotFound,
                    IsSuccessful = baseResponse.Success
                },
                ApiBadRequestResponse => new ResponseModel<TResult>
                {
                    Message = ((ApiBadRequestResponse)baseResponse).Message,
                    StatusCode = StatusCodes.Status400BadRequest,
                    IsSuccessful = baseResponse.Success
                },
                ApiUnauthorizedResponse => new ResponseModel<TResult>
                {
                    Message = ((ApiUnauthorizedResponse)baseResponse).Message,
                    StatusCode = StatusCodes.Status401Unauthorized,
                    IsSuccessful = baseResponse.Success
                },
                ApiForbiddenResponse => new ResponseModel<TResult>
                {
                    Message = ((ApiUnauthorizedResponse)baseResponse).Message,
                    StatusCode = StatusCodes.Status403Forbidden,
                    IsSuccessful = baseResponse.Success
                },
                ApiOkResponse<TResult> => new ResponseModel<TResult>
                {
                    StatusCode = ((ApiOkResponse<TResult>)baseResponse).StatucCode,
                    IsSuccessful = baseResponse.Success,
                    Data = baseResponse.GetResult<TResult>()
                },
                _ => throw new NotImplementedException()
            };
        }
    }
}
