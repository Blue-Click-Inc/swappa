using MediatR;
using Swappa.Shared.DTOs;

namespace Swappa.Server.Commands.Faq
{
    public class UpdateFaqCommand : IRequest<ResponseModel<string>>
    {
        public Guid Id { get; set; }
        public FaqToUpdateDto Request { get; set; } = new();
    }
}
