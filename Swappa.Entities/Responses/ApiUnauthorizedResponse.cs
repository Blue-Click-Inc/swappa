using System.Net;

namespace Swappa.Entities.Responses
{
    public abstract class ApiUnauthorizedResponse : ApiBaseResponse
    {
        public string? Message { get; set; }
        public int StatucCode { get; set; } = (int)HttpStatusCode.Unauthorized;

        public ApiUnauthorizedResponse(string? message) : base(false) =>
            Message = message;
    }
}
