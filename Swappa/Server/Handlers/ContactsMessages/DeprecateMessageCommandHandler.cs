using MediatR;
using Swappa.Data.Contracts;
using Swappa.Entities.Responses;
using Swappa.Server.Commands.ContactMessage;
using Swappa.Shared.DTOs;
using Swappa.Shared.Extensions;

namespace Swappa.Server.Handlers.ContactsMessages
{
    public class DeprecateMessageCommandHandler : IRequestHandler<DeprecateMessageCommand, ResponseModel<string>>
    {
        private readonly ApiResponseDto response;
        private readonly IRepositoryManager repository;

        public DeprecateMessageCommandHandler(ApiResponseDto response,
            IRepositoryManager repository)
        {
            this.response = response;
            this.repository = repository;
        }

        public async Task<ResponseModel<string>> Handle(DeprecateMessageCommand request, CancellationToken cancellationToken)
        {
            if (request.IsNull() || request.Id.IsEmpty())
            {
                return response.Process<string>(new BadRequestResponse("Invalid request"));
            }

            var message = await repository.ContactMessage.FindByConditionAsync(m => m.Id.Equals(request.Id));
            if (message.IsNull())
            {
                return response.Process<string>(new BadRequestResponse("Could not find the requested record"));
            }

            message.IsDeprecated = true;
            message.UpdatedAt = DateTime.UtcNow;
            await repository.ContactMessage.EditAsync(m => m.Id.Equals(request.Id), message);
            return response.Process<string>(new ApiOkResponse<string>("Message successfully deleted."));
        }
    }
}
