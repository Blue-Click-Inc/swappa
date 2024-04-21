using Microsoft.AspNetCore.Mvc;
using Swappa.Entities.Exceptions;
using Swappa.Entities.Responses;
using Swappa.Server.Controllers.Extensions;

namespace Swappa.Server.Controllers
{
    public class ApiControllerBase : ControllerBase
    {
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult ProcessResponse<TResult>(ApiBaseResponse baseResponse)
        {
            return baseResponse switch
            {
                ApiNotFoundResponse => NotFound(new ResponseModel<bool>
                {
                    Message = ((ApiNotFoundResponse)baseResponse).Message,
                    StatusCode = StatusCodes.Status404NotFound,
                    IsSuccessful = baseResponse.Success
                }),
                ApiBadRequestResponse => BadRequest(new ResponseModel<bool>
                {
                    Message = ((ApiBadRequestResponse)baseResponse).Message,
                    StatusCode = StatusCodes.Status400BadRequest,
                    IsSuccessful = baseResponse.Success
                }),
                ApiUnauthorizedResponse => Unauthorized(new ResponseModel<bool>
                {
                    Message = ((ApiUnauthorizedResponse)baseResponse).Message,
                    StatusCode = StatusCodes.Status401Unauthorized,
                    IsSuccessful = baseResponse.Success
                }),
                ApiOkResponse<TResult> => Ok(new ResponseModel<TResult>
                {
                    StatusCode = ((ApiOkResponse<TResult>)baseResponse).StatucCode,
                    IsSuccessful = baseResponse.Success,
                    Data = baseResponse.GetResult<TResult>()
                }),
                _ => throw new NotImplementedException()
            };
        }
    }
}
