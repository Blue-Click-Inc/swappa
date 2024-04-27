using MediatR;
using Microsoft.AspNetCore.Identity;
using Swappa.Entities.Models;
using Swappa.Entities.Responses;
using Swappa.Server.Commands.Role;
using Swappa.Server.Extensions;
using Swappa.Shared.DTOs;

namespace Swappa.Server.Handlers.Role
{
    public class DeleteRoleCommandHandler : IRequestHandler<DeleteRoleCommand, ResponseModel<string>>
    {
        private readonly RoleManager<AppRole> roleManager;
        private readonly UserManager<AppUser> userManager;
        private readonly ApiResponseDto response;

        public DeleteRoleCommandHandler(RoleManager<AppRole> roleManager,
            UserManager<AppUser> userManager,
            ApiResponseDto response)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.response = response;
        }

        public async Task<ResponseModel<string>> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
        {
            if (request == null || request.Id.IsEmpty())
                return response.Process<string>(new BadRequestResponse($"Invalid request parameter(s)."));

            var role = await roleManager.FindByIdAsync(request.Id.ToString());
            if (role == null)
                return response.Process<string>(new BadRequestResponse($"No role record found with Id: {request.Id}"));

            var userExists = (await userManager.GetUsersInRoleAsync(role.Name)).Any();
            if (userExists)
                return response.Process<string>(new BadRequestResponse($"Role, {role.Name} can not be delete because users exists in the role."));

            await roleManager.DeleteAsync(role);
            return response.Process<string>(new ApiOkResponse<string>($"Role, {role.Name} successfully deleted."));
        }
    }
}
