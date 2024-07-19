using MediatR;
using Microsoft.AspNetCore.Identity;
using Swappa.Data.Contracts;
using Swappa.Data.Services.Interfaces;
using Swappa.Entities.Models;
using Swappa.Entities.Responses;
using Swappa.Server.Commands.Account;
using Swappa.Shared.DTOs;
using Swappa.Shared.Extensions;

namespace Swappa.Server.Handlers.Account
{
    public class AddToRoleCommandHandler : IRequestHandler<AddToRoleCommand, ResponseModel<string>>
    {
        private readonly UserManager<AppUser> userManager;
        private readonly RoleManager<AppRole> roleManager;
        private readonly ApiResponseDto response;
        private readonly IRepositoryManager repository;

        public AddToRoleCommandHandler(UserManager<AppUser> userManager,
            RoleManager<AppRole> roleManager,
            ApiResponseDto response, IRepositoryManager repository)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.response = response;
            this.repository = repository;
        }

        public async Task<ResponseModel<string>> Handle(AddToRoleCommand request, CancellationToken cancellationToken)
        {
            if(request == null || request.UserId.IsEmpty())
            {
                return response.Process<string>(new BadRequestResponse("Invalid request parameters."));
            }

            var userHasRights = await repository.Common.HasEqualOrHigherRole(request.Role);
            if (!userHasRights)
            {
                return response.Process<string>(new BadRequestResponse($"You have no permission to assign a user to {request.Role.GetDescription()} role."));
            }

            var user = await userManager.FindByIdAsync(request.UserId.ToString());
            if(user == null)
            {
                return response.Process<string>(new NotFoundResponse($"No user found with Id: {request.UserId}."));
            }

            var exisingRole = await userManager.GetRolesAsync(user);

            var role = await roleManager.FindByNameAsync(request.Role.ToString());
            if(role == null)
            {
                return response.Process<string>(new NotFoundResponse($"No role with the Id: {request.Role.GetDescription()}."));
            }

            var result = await userManager.AddToRoleAsync(user, role.Name);
            if (result.Succeeded)
            {
                if(exisingRole.IsNotNullOrEmpty())
                {
                    await userManager.RemoveFromRoleAsync(user, exisingRole.First());
                }
                return response.Process<string>(new ApiOkResponse<string>($"User successfully added to {role.Name} role."));
            }

            return response.Process<string>(new BadRequestResponse(result.Errors.FirstOrDefault()?.Description ?? string.Empty));
        }
    }
}
