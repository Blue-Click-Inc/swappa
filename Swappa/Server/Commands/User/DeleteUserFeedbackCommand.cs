using MediatR;
using Swappa.Shared.DTOs;

namespace Swappa.Server.Commands.User
{
    public class DeleteUserFeedbackCommand : IRequest<ResponseModel<string>>
    {
        public Guid Id { get; set; }
    }
}
