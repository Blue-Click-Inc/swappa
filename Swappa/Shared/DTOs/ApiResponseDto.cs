using Microsoft.AspNetCore.Http;
using Swappa.Entities.Responses;
using Swappa.Shared.DTOs.Extensions;

namespace Swappa.Shared.DTOs
{
    public class ApiResponseDto
    {
        public ResponseModel<TResult> Process<TResult>(ApiBaseResponse baseResponse)
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
                    Message = ((ApiForbiddenResponse)baseResponse).Message,
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
