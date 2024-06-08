using MediatR;
using Swappa.Shared.DTOs;

namespace Swappa.Server.Commands.User
{
    public class SendUserFeedbackCommand : IRequest<ResponseModel<string>>
    {
        public FeedbackForAddDto Request { get; set; } = new();
    }
}
