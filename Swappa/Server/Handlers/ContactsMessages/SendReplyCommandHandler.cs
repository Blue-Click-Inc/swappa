using MediatR;
using Microsoft.AspNetCore.Identity;
using Swappa.Data.Contracts;
using Swappa.Data.Services.Interfaces;
using Swappa.Entities.Models;
using Swappa.Entities.Responses;
using Swappa.Server.Commands.ContactMessage;
using Swappa.Shared.DTOs;
using Swappa.Shared.Extensions;

namespace Swappa.Server.Handlers.ContactsMessages
{
    public class SendReplyCommandHandler : IRequestHandler<SendReplyCommand, ResponseModel<string>>
    {
        private readonly ApiResponseDto response;
        private readonly IRepositoryManager repository;
        private readonly IServiceManager service;
        private readonly UserManager<AppUser> userManager;

        public SendReplyCommandHandler(ApiResponseDto response,
            IRepositoryManager repository, IServiceManager service,
            UserManager<AppUser> userManager)
        {
            this.response = response;
            this.repository = repository;
            this.service = service;
            this.userManager = userManager;
        }

        public async Task<ResponseModel<string>> Handle(SendReplyCommand request, CancellationToken cancellationToken)
        {
            if(request.IsNull() || request.Message.IsNull() || request.Origin.IsNull())
            {
                return response.Process<string>(new BadRequestResponse("Invalid request."));
            }

            var userId = repository.Common.GetLoggedInUserId();
            if (userId.IsNullOrEmpty())
            {
                return response.Process<string>(new BadRequestResponse("You are not authorized to perform this operation"));
            }

            var user = await userManager.FindByIdAsync(userId);
            if (user.IsNull())
            {
                return response.Process<string>(new BadRequestResponse("You are not authorized to perform this operation"));
            }

            var baseUrl = request.Origin.BuildBaseUrl();
            if(request.Message.Email.IsNotNullOrEmpty() &&
                request.Message.Name.IsNotNullOrEmpty() &&
                baseUrl.IsNotNullOrEmpty())
            {
                var message = Statics.GetReplyMessageTemplate(baseUrl, request.Message.Name, user.Name, request.Message.Body);
                if(message.IsNotNullOrEmpty())
                {
                    var res = await service.Notify.SendAsync(request.Message.Email, message, request.Message.Subject);
                    if (res.HasValue && res.Value)
                    {
                        return response.Process<string>(new ApiOkResponse<string>("Response successfully sent."));
                    }
                    else
                    {
                        return response.Process<string>(new BadRequestResponse("An error occurred while sending your reply. Please try again!"));
                    }
                }
                else
                {
                    return response.Process<string>(new BadRequestResponse("An unxpected error occurred. Please reach out to the support team if issue persists."));
                }
            }
            else
            {
                return response.Process<string>(new BadRequestResponse("One or more validation error occurred. Please review your input and try again."));
            }  
        }
    }
}
