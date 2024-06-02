using System.Net;

namespace Swappa.Entities.Responses
{
    public sealed class ApiOkResponse<TResult> : ApiBaseResponse
    {
        public TResult Result { get; set; }
        public int StatucCode { get; set; } = (int)HttpStatusCode.OK;

        public ApiOkResponse(TResult result) : base(true) =>
            Result = result;
    }
}
