using Swappa.Shared.DTOs.Interfaces;

namespace Swappa.Shared.DTOs
{
    public record IdDto : IBaseIdDto
    {
        public Guid Id { get; set; }
    }
}