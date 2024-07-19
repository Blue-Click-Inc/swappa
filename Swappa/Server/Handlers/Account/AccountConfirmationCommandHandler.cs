using MediatR;
using Microsoft.AspNetCore.Identity;
using Swappa.Data.Contracts;
using Swappa.Data.Services.Interfaces;
using Swappa.Entities.Enums;
using Swappa.Entities.Models;
using Swappa.Entities.Responses;
using Swappa.Server.Commands.Account;
using Swappa.Shared.DTOs;

namespace Swappa.Server.Handlers.Account
{
    public class AccountConfirmationCommandHandler : IRequestHandler<ConfirmationCommand, ResponseModel<string>>
    {
        private readonly UserManager<AppUser> userManager;
        private readonly IRepositoryManager repository;
        private readonly ApiResponseDto response;
        private readonly IServiceManager service;

        public AccountConfirmationCommandHandler(UserManager<AppUser> userManager,
            IRepositoryManager repository, ApiResponseDto response,
            IServiceManager service)
        {
            this.userManager = userManager;
            this.repository = repository;
            this.response = response;
            this.service = service;
        }

        public async Task<ResponseModel<string>> Handle(ConfirmationCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Token))
                return response.Process<string>(new BadRequestResponse("Invalid token provided."));

            var tokenFromDb = await repository.Token.FindAsync(t => t.Value.Equals(request.Token));
            if (tokenFromDb == null)
            {
                return response.Process<string>(new BadRequestResponse("Invalid or expired token provided."));
            }

            var user = await userManager.FindByIdAsync(tokenFromDb.UserId.ToString());
            if (user == null)
                return response.Process<string>(new BadRequestResponse("No user found."));

            if (DateTime.Now < tokenFromDb.ExpiresAt)
            {
                if (tokenFromDb.Type == TokenType.AccountConfirmation)
                {
                    user.EmailConfirmed = true;
                    user.UpdatedOn = DateTime.UtcNow;
                    user.Status = Status.Active;
                    await userManager.UpdateAsync(user);

                    return response.Process<string>(new ApiOkResponse<string>("Account confirmation successful. Please proceed to login"));
                }
            }

            var result = await service.Notify.SendAccountEmailAsync(user, request.Origin, TokenType.AccountConfirmation);
            result.Message = "Token expired. Another link has been sent to your email. Please reconfirm to activate your account.";
            return result;
        }
    }
}
