namespace Swappa.Entities.Responses
{
    public sealed class ForbiddenResponse : ApiForbiddenResponse
    {
        public ForbiddenResponse(string message) : base(message) { }
    }
}
