using System.Net;

namespace Swappa.Entities.Responses
{
    public abstract class ApiNotFoundResponse : ApiBaseResponse
    {
        public string? Message { get; set; }
        public int StatucCode { get; set; } = (int)HttpStatusCode.NotFound;

        public ApiNotFoundResponse(string? message) : base(false) =>
            this.Message = message;
    }
}
