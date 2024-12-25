using MediatR;
using Swappa.Shared.DTOs;

namespace Swappa.Server.Commands.Faq
{
    public class CreateFaqsCommand : IRequest<ResponseModel<string>>
    {
        public List<FaqToCreateDto> Requests { get; set; } = new();
    }
}
