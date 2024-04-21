using System.Text.Json;

namespace Swappa.Shared.DTOs
{
    public class ResponseModel<T>
    {
        public int StatusCode { get; set; }
        public bool IsSuccessful { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }
        public override string ToString() => JsonSerializer.Serialize(this);
    }
}
