using System.Net;

namespace Swappa.Entities.Responses
{
    public abstract class ApiForbiddenResponse : ApiBaseResponse
    {
        public string? Message { get; set; }
        public int StatucCode { get; set; } = (int)HttpStatusCode.Forbidden;

        public ApiForbiddenResponse(string? message) : base(false) =>
            Message = message;
    }
}
