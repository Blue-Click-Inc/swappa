using MediatR;
using Microsoft.AspNetCore.Identity;
using Swappa.Data.Services.Interfaces;
using Swappa.Entities.Enums;
using Swappa.Entities.Models;
using Swappa.Entities.Responses;
using Swappa.Server.Commands.Account;
using Swappa.Shared.DTOs;

namespace Swappa.Server.Handlers.Account
{
    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, ResponseModel<string>>
    {
        private readonly UserManager<AppUser> userManager;
        private readonly ApiResponseDto response;
        private readonly IServiceManager service;

        public ResetPasswordCommandHandler(UserManager<AppUser> userManager,
            ApiResponseDto response, IServiceManager service)
        {
            this.userManager = userManager;
            this.response = response;
            this.service = service;
        }

        public async Task<ResponseModel<string>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByEmailAsync(request.Email);

            if (user == null || !user.EmailConfirmed)
                return response.Process<string>(new BadRequestResponse($"Could not find a user with the email: {request.Email}, or account not confirmed yet."));

            return await service.Notify.SendAccountEmailAsync(user, request.Origin, TokenType.PasswordReset);
        }
    }
}
