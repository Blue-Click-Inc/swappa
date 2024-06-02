namespace Swappa.Entities.Responses
{
    public sealed class BadRequestResponse : ApiBadRequestResponse
    {
        public BadRequestResponse(string message) : base(message) { }
    }
}
