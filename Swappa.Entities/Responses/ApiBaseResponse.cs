namespace Swappa.Entities.Responses
{
    public abstract class ApiBaseResponse
    {
        public bool Success { get; set; }

        public ApiBaseResponse(bool success) =>
            Success = success;
    }
}
