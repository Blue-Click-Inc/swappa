namespace Swappa.Entities.Responses
{
    public sealed class UnauthorizedResponse : ApiUnauthorizedResponse
    {
        public UnauthorizedResponse(string message) : base(message) { }
    }
}
