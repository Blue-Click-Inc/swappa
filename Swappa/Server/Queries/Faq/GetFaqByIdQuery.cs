using MediatR;
using Swappa.Shared.DTOs;

namespace Swappa.Server.Queries.Faq
{
    public class GetFaqByIdQuery: IRequest<ResponseModel<FaqToReturnDto>>
    {
        public Guid Id { get; set; }
    }
}
