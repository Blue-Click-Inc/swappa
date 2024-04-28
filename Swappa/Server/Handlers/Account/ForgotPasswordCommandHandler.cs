using MediatR;
using Microsoft.AspNetCore.Identity;
using Swappa.Data.Contracts;
using Swappa.Entities.Enums;
using Swappa.Entities.Models;
using Swappa.Entities.Responses;
using Swappa.Server.Commands.Account;
using Swappa.Server.Validations.Account;
using Swappa.Shared.DTOs;

namespace Swappa.Server.Handlers.Account
{
    public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand, ResponseModel<string>>
    {
        private readonly UserManager<AppUser> userManager;
        private readonly ApiResponseDto response;
        private readonly IRepositoryManager repository;

        public ForgotPasswordCommandHandler(UserManager<AppUser> userManager, 
            ApiResponseDto response, IRepositoryManager repository)
        {
            this.userManager = userManager;
            this.response = response;
            this.repository = repository;
        }

        public async Task<ResponseModel<string>> Handle(ForgotPasswordCommand command, CancellationToken cancellationToken)
        {
            var validate = (new ForgotPasswordValidator().Validate(command));
            if (!validate.IsValid)
                return response.Process<string>(new BadRequestResponse($"{validate.Errors.FirstOrDefault()?.ErrorMessage}"));

            var tokenFromDb = await repository.Token.FindAsync(t => t.Value.Equals(command.Request.Token));
            if (tokenFromDb == null)
                return response.Process<string>(new BadRequestResponse("Invalid token provided"));

            var user = await userManager.FindByIdAsync(tokenFromDb.UserId.ToString());
            if (user == null)
                return response.Process<string>(new BadRequestResponse("No user found."));

            if (DateTime.Now < tokenFromDb.ExpiresAt)
            {
                if (tokenFromDb.Type == TokenType.PasswordReset)
                {
                    var result = await userManager.ResetPasswordAsync(user, Uri.UnescapeDataString(command.Request.Token), command.Request.NewPassword);
                    if (!result.Succeeded)
                        return response.Process<string>(new BadRequestResponse($"{result.Errors.FirstOrDefault()?.Description}"));

                    return response.Process<string>(new ApiOkResponse<string>("Password successfully changed. Please proceed to login."));
                }
            }

            //TODO: Resend password reset email.
            return response.Process<string>(new BadRequestResponse("Token expired. Another token sent to your email. Please click on the link to change your password."));
        }
    }
}
