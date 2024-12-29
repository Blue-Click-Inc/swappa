using MediatR;
using Swappa.Shared.DTOs;

namespace Swappa.Server.Commands.Faq
{
    public class DeleteFaqCommand: IRequest<ResponseModel<string>>
    {
        public Guid Id { get; set; }
    }
}
