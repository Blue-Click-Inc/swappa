using Mailjet.Client.Resources;
using MediatR;
using Swappa.Data.Contracts;
using Swappa.Entities.Responses;
using Swappa.Server.Commands.ContactMessage;
using Swappa.Shared.DTOs;
using Swappa.Shared.Extensions;

namespace Swappa.Server.Handlers.ContactsMessages
{
    public class DeprecateSelectedMessagesCommandHandler : IRequestHandler<DeprecateSelectedMessagesCommand, ResponseModel<string>>
    {
        private readonly ApiResponseDto response;
        private readonly IRepositoryManager repository;

        public DeprecateSelectedMessagesCommandHandler(ApiResponseDto response,
            IRepositoryManager repository)
        {
            this.response = response;
            this.repository = repository;
        }

        public async Task<ResponseModel<string>> Handle(DeprecateSelectedMessagesCommand request, CancellationToken cancellationToken)
        {
            if (request.IsNull() || request.SelectedIds.IsNullOrEmpty())
            {
                return response.Process<string>(new BadRequestResponse("Invalid request."));
            }

            var messages = await Task.Run(() => repository.ContactMessage
                .FindAsQueryable(m => request.SelectedIds.Contains(m.Id))
                .ToList());
            if (messages.IsNullOrEmpty())
            {
                return response.Process<string>(new BadRequestResponse("Could not find the requested records"));
            }

            messages.ForEach(m =>
            {
                m.IsDeprecated = true;
                m.UpdatedAt = DateTime.UtcNow;
            });
            await repository.ContactMessage.EditManyAsync(messages);
            return response.Process<string>(new ApiOkResponse<string>("Selected message successfully deleted."));
        }
    }
}
