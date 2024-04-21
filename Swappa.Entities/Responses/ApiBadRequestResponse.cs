using System.Net;

namespace Swappa.Entities.Responses
{
    public abstract class ApiBadRequestResponse : ApiBaseResponse
    {
        public string? Message { get; set; }
        public int StatucCode { get; set; } = (int)HttpStatusCode.BadRequest;

        public ApiBadRequestResponse(string? message) : base(false) =>
            this.Message = message;
    }
}
