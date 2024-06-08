using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Swappa.Data.Contracts;
using Swappa.Entities.Models;
using Swappa.Entities.Responses;
using Swappa.Server.Commands.User;
using Swappa.Server.Validations.User;
using Swappa.Shared.DTOs;

namespace Swappa.Server.Handlers.User
{
    public class SendUserFeedbackCommandHandler : IRequestHandler<SendUserFeedbackCommand, ResponseModel<string>>
    {
        private readonly IRepositoryManager repository;
        private readonly UserManager<AppUser> userManager;
        private readonly IMapper mapper;
        private readonly ApiResponseDto response;

        public SendUserFeedbackCommandHandler(IRepositoryManager repository,
            UserManager<AppUser> userManager, IMapper mapper, ApiResponseDto response)
        {
            this.repository = repository;
            this.userManager = userManager;
            this.mapper = mapper;
            this.response = response;
        }

        public async Task<ResponseModel<string>> Handle(SendUserFeedbackCommand command, CancellationToken cancellationToken)
        {
            if(command == null || command.Request == null)
            {
                return response.Process<string>(new BadRequestResponse($"Invalid request parameters."));
            }

            var validator = new SendUserFeedbackValidator();
            var validationResult = validator.Validate(command.Request);
            if (!validationResult.IsValid)
            {
                return response.Process<string>(new BadRequestResponse(validationResult.Errors?.FirstOrDefault()?.ErrorMessage ?? string.Empty));
            }

            var user = await userManager.FindByEmailAsync(command.Request.UserEmail);
            if(user == null)
            {
                return response.Process<string>(new BadRequestResponse($"Feedback sending failed. No user found with the email: {command.Request.UserEmail}"));
            }

            var feedback = mapper.Map<UserFeedback>(command.Request);
            feedback.UserName = user.Name;
            return response.Process<string>(new ApiOkResponse<string>("Feedback successfully sent."));
        }
    }
}
