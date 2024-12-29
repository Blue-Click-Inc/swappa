using MediatR;
using Swappa.Data.Contracts;
using Swappa.Entities.Models;
using Swappa.Entities.Responses;
using Swappa.Server.Commands.ContactMessage;
using Swappa.Server.Validations.ContactMessage;
using Swappa.Shared.DTOs;
using Swappa.Shared.Extensions;

namespace Swappa.Server.Handlers.ContactsMessages
{
    public class CreateContactMessageCommandHandler : IRequestHandler<CreateContactMessageCommand, ResponseModel<string>>
    {
        private readonly ApiResponseDto response;
        private readonly IRepositoryManager repository;

        public CreateContactMessageCommandHandler(ApiResponseDto response,
            IRepositoryManager repository)
        {
            this.response = response;
            this.repository = repository;
        }

        public async Task<ResponseModel<string>> Handle(CreateContactMessageCommand request, CancellationToken cancellationToken)
        {
            if (request.IsNull())
            {
                return response.Process<string>(new BadRequestResponse("Invalid request"));
            }

            var validator = (new ContactMessageValidator()).Validate(request);
            if (!validator.IsValid)
            {
                var message = validator.Errors.FirstOrDefault()?.ErrorMessage ?? "Invalid request";
                return response.Process<string>(new BadRequestResponse(message));
            }

            var contactMessage = new ContactMessage
            {
                Name = request.FullName,
                Message = request.Message,
                EmailAddress = request.Email,
                PhoneNumber = request.Phone,

            };

            await repository.ContactMessage.AddAsync(contactMessage);
            return response.Process<string>(new ApiOkResponse<string>("Your message has been sent. Someone will reach out to you as soon as possible."));
        }
    }
}
