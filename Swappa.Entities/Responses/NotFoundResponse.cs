namespace Swappa.Entities.Responses
{
    public sealed class NotFoundResponse : ApiNotFoundResponse
    {
        public NotFoundResponse(string message) : base(message) { }
    }
}
