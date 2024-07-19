using MediatR;
using Microsoft.AspNetCore.Identity;
using Swappa.Entities.Models;
using Swappa.Entities.Responses;
using Swappa.Server.Commands.Account;
using Swappa.Server.Validations.Account;
using Swappa.Shared.DTOs;

namespace Swappa.Server.Handlers.Account
{
    public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, ResponseModel<string>>
    {
        private readonly UserManager<AppUser> userManager;
        private readonly ApiResponseDto response;

        public ChangePasswordCommandHandler(UserManager<AppUser> userManager, ApiResponseDto response)
        {
            this.userManager = userManager;
            this.response = response;
        }

        public async Task<ResponseModel<string>> Handle(ChangePasswordCommand command, CancellationToken cancellationToken)
        {
            var validator = (new ChangePasswordValidator()).Validate(command);

            if (!validator.IsValid)
                return response.Process<string>(new BadRequestResponse($"{validator.Errors.FirstOrDefault()?.ErrorMessage}"));

            var user = await userManager.FindByIdAsync(command.Id.ToString());
            if (user == null)
                return response.Process<string>(new BadRequestResponse($"No user found with Id: {command.Id}"));

            var result = await userManager.ChangePasswordAsync(user, command.Request.CurrentPassword, command.Request.NewPassword);
            if (!result.Succeeded)
                return response.Process<string>(new BadRequestResponse($"{result.Errors.FirstOrDefault()?.Description}"));

            return response.Process<string>(new ApiOkResponse<string>("Password changed successfully."));
        }
    }
}
