using MediatR;
using Swappa.Data.Contracts;
using Swappa.Entities.Responses;
using Swappa.Server.Queries.ContactMessages;
using Swappa.Shared.DTOs;
using Swappa.Shared.Extensions;

namespace Swappa.Server.Handlers.ContactsMessages
{
    public class GetContactMessageByIdQueryHandler : IRequestHandler<GetContactMessageByIdQuery, ResponseModel<ContactMessageToReturnDto>>
    {
        private readonly ApiResponseDto response;
        private readonly IRepositoryManager repository;

        public GetContactMessageByIdQueryHandler(ApiResponseDto response,
            IRepositoryManager repository)
        {
            this.response = response;
            this.repository = repository;
        }

        public async Task<ResponseModel<ContactMessageToReturnDto>> Handle(GetContactMessageByIdQuery request, CancellationToken cancellationToken)
        {
            if (request.IsNull() || request.Id.IsEmpty())
            {
                return response.Process<ContactMessageToReturnDto>(new BadRequestResponse("Invalid request"));
            }

            var message = await repository.ContactMessage.FindByConditionAsync(m => m.Id.Equals(request.Id));
            if (message.IsNull())
            {
                return response.Process<ContactMessageToReturnDto>(new BadRequestResponse("Could not find the requested record"));
            }

            message.IsRead = !message.IsRead;
            message.UpdatedAt = DateTime.UtcNow;
            await repository.ContactMessage.EditAsync(c => c.Id.Equals(request.Id), message);
            return response.Process<ContactMessageToReturnDto>(new ApiOkResponse<ContactMessageToReturnDto>(new ContactMessageToReturnDto
            {
                Id = message.Id,
                Name = message.Name,
                Message = message.Message,
                Email = message.EmailAddress,
                Phone = message.PhoneNumber,
                IsRead = message.IsRead,
                DateAdded = message.CreatedAt
            }));
        }
    }
}
