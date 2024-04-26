using MediatR;
using Microsoft.AspNetCore.Identity;
using Swappa.Data.Contracts;
using Swappa.Entities.Enums;
using Swappa.Entities.Models;
using Swappa.Entities.Responses;
using Swappa.Server.Commands.Account;
using Swappa.Shared.DTOs;

namespace Swappa.Server.Handlers.Account
{
    public class AccountConfirmationHandler : IRequestHandler<ConfirmationCommand, ResponseModel<string>>
    {
        private readonly UserManager<AppUser> userManager;
        private readonly IRepositoryManager repository;
        private readonly ApiResponseDto response;

        public AccountConfirmationHandler(UserManager<AppUser> userManager,
            IRepositoryManager repository,
            ApiResponseDto response)
        {
            this.userManager = userManager;
            this.repository = repository;
            this.response = response;
        }

        public async Task<ResponseModel<string>> Handle(ConfirmationCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Token))
                return response.Process<string>(new BadRequestResponse("Invalid token provided."));

            var tokenFromDb = await repository.Token.FindAsync(t => t.Value.Equals(request.Token));
            if (tokenFromDb == null)
            {
                //TODO: Resend another confirmation token
                return response.Process<string>(new BadRequestResponse("Invalid token provided"));
            }

            var user = await userManager.FindByIdAsync(tokenFromDb.UserId.ToString());
            if (user == null)
                return response.Process<string>(new BadRequestResponse("No user found."));

            if (DateTime.Now < tokenFromDb.ExpiresAt)
            {
                if (tokenFromDb.Type == TokenType.AccountConfirmation)
                {
                    user.EmailConfirmed = true;
                    user.UpdatedOn = DateTime.Now;
                    user.Status = Status.Active;
                    await userManager.UpdateAsync(user);

                    return response.Process<string>(new ApiOkResponse<string>("Account confirmation successful. Please proceed to login"));
                }
            }

            //TODO: Resent confirmation email.
            return response.Process<string>(new BadRequestResponse("Token expired. Another token sent to your email. Please reconfirm to activate your account."));
        }
    }
}
