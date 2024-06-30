using MediatR;
using Microsoft.AspNetCore.Identity;
using Swappa.Data.Services.Interfaces;
using Swappa.Entities.Models;
using Swappa.Entities.Responses;
using Swappa.Server.Commands.Account;
using Swappa.Shared.DTOs;
using Swappa.Shared.Extensions;

namespace Swappa.Server.Handlers.Account
{
    public class RemoveFromRoleCommandHandler : IRequestHandler<RemoveFromRoleCommand, ResponseModel<string>>
    {
        private readonly UserManager<AppUser> userManager;
        private readonly RoleManager<AppRole> roleManager;
        private readonly ApiResponseDto response;
        private readonly ICommon common;

        public RemoveFromRoleCommandHandler(UserManager<AppUser> userManager,
            RoleManager<AppRole> roleManager,
            ApiResponseDto response,
            ICommon common)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.response = response;
            this.common = common;
        }
        public async Task<ResponseModel<string>> Handle(RemoveFromRoleCommand request, CancellationToken cancellationToken)
        {
            if (request == null || request.UserId.IsEmpty())
            {
                return response.Process<string>(new BadRequestResponse("Invalid request parameters."));
            }

            var userHasRights = await common.HasEqualOrHigherRole(request.Role);
            if (!userHasRights)
            {
                return response.Process<string>(new BadRequestResponse($"You have no permission to remove a user from {request.Role.GetDescription()} role."));
            }

            var user = await userManager.FindByIdAsync(request.UserId.ToString());
            if (user == null)
            {
                return response.Process<string>(new NotFoundResponse($"No user found with Id: {request.UserId}."));
            }

            var role = await roleManager.FindByNameAsync(request.Role.ToString());
            if (role == null)
            {
                return response.Process<string>(new NotFoundResponse($"No role with the Id: {request.Role.GetDescription()}."));
            }

            var result = await userManager.RemoveFromRoleAsync(user, role.Name);
            if (result.Succeeded)
            {
                return response.Process<string>(new ApiOkResponse<string>($"User successfully removed from {role.Name} role."));
            }

            return response.Process<string>(new BadRequestResponse(result.Errors.FirstOrDefault()?.Description ?? string.Empty));
        }
    }
}
