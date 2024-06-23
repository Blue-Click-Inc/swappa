using MediatR;
using Swappa.Shared.DTOs;

namespace Swappa.Server.Commands.Location
{
    public class DeleteLocationCommand : IRequest<ResponseModel<string>>
    {
        public Guid Id { get; set; }
    }
}
