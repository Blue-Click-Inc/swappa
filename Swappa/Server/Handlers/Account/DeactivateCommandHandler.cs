using MediatR;
using Microsoft.AspNetCore.Identity;
using Swappa.Entities.Enums;
using Swappa.Entities.Models;
using Swappa.Entities.Responses;
using Swappa.Server.Commands.Account;
using Swappa.Shared.Extensions;
using Swappa.Shared.DTOs;

namespace Swappa.Server.Handlers.Account
{
    public class DeactivateCommandHandler : IRequestHandler<DeactivateCommand, ResponseModel<string>>
    {
        private readonly UserManager<AppUser> userManager;
        private readonly ApiResponseDto response;

        public DeactivateCommandHandler(UserManager<AppUser> userManager, 
            ApiResponseDto response)
        {
            this.userManager = userManager;
            this.response = response;
        }

        public async Task<ResponseModel<string>> Handle(DeactivateCommand command, CancellationToken cancellationToken)
        {
            if(command == null || command.Id.IsEmpty())
                return response.Process<string>(new BadRequestResponse("Id can not be null or empty."));

            var user = await userManager.FindByIdAsync(command.Id.ToString());
            if(user == null)
                return response.Process<string>(new BadRequestResponse($"No user found with the Id: {command.Id}."));

            user.Status = Status.Inactive;
            user.IsDeprecated = true;
            user.UpdatedOn = DateTime.Now;
            user.DeactivatedOn = DateTime.Now;
            await userManager.UpdateAsync(user);

            return response
                .Process<string>(new ApiOkResponse<string>("Account deactivation successful. Please log back in within the next 90 days to continue using your account."));
        }
    }
}
