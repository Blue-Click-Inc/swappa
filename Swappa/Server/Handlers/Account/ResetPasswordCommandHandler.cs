using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Primitives;
using Swappa.Data.Contracts;
using Swappa.Data.Services.Interfaces;
using Swappa.Entities.Enums;
using Swappa.Entities.Models;
using Swappa.Entities.Responses;
using Swappa.Server.Commands.Account;
using Swappa.Server.Extensions;
using Swappa.Shared.DTOs;

namespace Swappa.Server.Handlers.Account
{
    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, ResponseModel<string>>
    {
        private readonly UserManager<AppUser> userManager;
        private readonly ApiResponseDto response;
        private readonly IRepositoryManager repository;
        private readonly INotify notify;

        public ResetPasswordCommandHandler(UserManager<AppUser> userManager,
            ApiResponseDto response, IRepositoryManager repository, INotify notify)
        {
            this.userManager = userManager;
            this.response = response;
            this.repository = repository;
            this.notify = notify;
        }

        public async Task<ResponseModel<string>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByEmailAsync(request.Email);

            if (user == null || !user.EmailConfirmed)
                return response.Process<string>(new BadRequestResponse($"Could not find a user with the email: {request.Email}, or account not confirmed yet."));

            return await SendPasswordResetEmail(user, request.Origin);
        }

        private async Task<ResponseModel<string>> SendPasswordResetEmail(AppUser user, StringValues origin)
        {
            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            var url = origin.BuildUrl(token);
            var message = Statics.GetPasswordResetTemplate(url);

            var success = await notify.SendAsync(user.Email, message, "Password Reset");
            if (success.GetValueOrDefault())
            {
                var tokenToAdd = new Token
                {
                    UserId = user.Id,
                    Type = TokenType.AccountConfirmation,
                    Value = token
                };
                await repository.Token.AddAsync(tokenToAdd);
                return response
                    .Process<string>(new ApiOkResponse<string>("Password reset successful. Please check your email to change mail to set a new password password."));
            }

            return response
                .Process<string>(new ApiOkResponse<string>("Password reset failed. Could not send reset toekn. Please try again later."));
        }
    }
}
