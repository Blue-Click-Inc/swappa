using MediatR;
using Swappa.Data.Contracts;
using Swappa.Entities.Responses;
using Swappa.Server.Commands.ContactMessage;
using Swappa.Shared.DTOs;
using Swappa.Shared.Extensions;

namespace Swappa.Server.Handlers.ContactsMessages
{
    public class ToggleContactMessageIsReadCommandHandler : IRequestHandler<ToggleContactMessageIsReadCommand, ResponseModel<bool>>
    {
        private readonly ApiResponseDto response;
        private readonly IRepositoryManager repository;

        public ToggleContactMessageIsReadCommandHandler(ApiResponseDto response,
            IRepositoryManager repository)
        {
            this.response = response;
            this.repository = repository;
        }

        public async Task<ResponseModel<bool>> Handle(ToggleContactMessageIsReadCommand request, CancellationToken cancellationToken)
        {
            if (request.IsNull() || request.Id.IsEmpty())
            {
                return response.Process<bool>(new BadRequestResponse("Invalid request"));
            }

            var message = await repository.ContactMessage.FindByConditionAsync(m => m.Id.Equals(request.Id));
            if (message.IsNull())
            {
                return response.Process<bool>(new BadRequestResponse("Could not find the requested record"));
            }

            message.IsRead = !message.IsRead;
            message.UpdatedAt = DateTime.UtcNow;
            await repository.ContactMessage.EditAsync(m => m.Id.Equals(request.Id), message);
            return response.Process<bool>(new ApiOkResponse<bool>(true));
        }
    }
}
