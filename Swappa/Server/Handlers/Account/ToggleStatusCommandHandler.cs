using MediatR;
using Microsoft.AspNetCore.Identity;
using Swappa.Data.Contracts;
using Swappa.Entities.Enums;
using Swappa.Entities.Models;
using Swappa.Entities.Responses;
using Swappa.Server.Commands.Account;
using Swappa.Shared.Extensions;
using Swappa.Shared.DTOs;

namespace Swappa.Server.Handlers.Account
{
    public class ToggleStatusCommandHandler : IRequestHandler<ToggleStatusCommand, ResponseModel<string>>
    {
        private readonly UserManager<AppUser> userManager;
        private readonly ApiResponseDto response;
        private readonly IRepositoryManager repository;

        public ToggleStatusCommandHandler(UserManager<AppUser> userManager, 
            ApiResponseDto response, IRepositoryManager repository)
        {
            this.userManager = userManager;
            this.response = response;
            this.repository = repository;
        }

        public async Task<ResponseModel<string>> Handle(ToggleStatusCommand request, CancellationToken cancellationToken)
        {
            if (request.Id.IsEmpty())
                return response.Process<string>(new BadRequestResponse("Id can not be null or empty."));

            var user = await userManager.FindByIdAsync(request.Id.ToString());
            if (user == null)
                return response.Process<string>(new BadRequestResponse($"No user found with the Id: {request.Id}"));

            if (repository.User.GetLoogedInUserId().Equals(user.Id))
                return response.Process<string>(new ForbiddenResponse("You are not allowed to carry out this operation!!!"));

            user.Status = user.Status == Status.Active ? Status.Inactive : Status.Active;
            await userManager.UpdateAsync(user);

            return response.Process<string>(new ApiOkResponse<string>("User status successfully updated."));
        }
    }
}
