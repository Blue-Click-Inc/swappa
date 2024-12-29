using MediatR;
using Swappa.Shared.DTOs;

namespace Swappa.Server.Commands.Faq
{
    public class CreateFaqCommand : FaqToCreateDto, IRequest<ResponseModel<string>>
    {
    }
}
