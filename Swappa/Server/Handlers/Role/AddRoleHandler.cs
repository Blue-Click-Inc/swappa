using MediatR;
using Microsoft.AspNetCore.Identity;
using Swappa.Entities.Models;
using Swappa.Entities.Responses;
using Swappa.Server.Commands.Role;
using Swappa.Shared.DTOs;

namespace Swappa.Server.Handlers.Role
{
    public class AddRoleHandler : IRequestHandler<AddRoleCommand, ResponseModel<string>>
    {
        private readonly RoleManager<AppRole> roleManager;
        private readonly ApiResponseDto response;

        public AddRoleHandler(RoleManager<AppRole> roleManager,
            ApiResponseDto response)
        {
            this.roleManager = roleManager;
            this.response = response;
        }

        public async Task<ResponseModel<string>> Handle(AddRoleCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.RoleName))
                return response.Process<string>(new BadRequestResponse("Role name is required."));

            var existing = await roleManager.RoleExistsAsync(request.RoleName);
            if (existing)
                return response.Process<string>(new BadRequestResponse($"Role {request.RoleName} already exists."));

            var result = await roleManager.CreateAsync(new AppRole
            {
                Name = request.RoleName,
                NormalizedName = request.RoleName.ToUpper()
            });

            if (!result.Succeeded)
                return response
                    .Process<string>(new BadRequestResponse($"Operation failed! {result.Errors.FirstOrDefault()?.Description}"));

            return response.Process<string>(new ApiOkResponse<string>($"Role {request.RoleName} successfully added."));
        }
    }
}
