using MediatR;
using Microsoft.AspNetCore.Identity;
using Swappa.Entities.Models;
using Swappa.Entities.Responses;
using Swappa.Server.Commands.Role;
using Swappa.Shared.DTOs;
using Swappa.Shared.Extensions;

namespace Swappa.Server.Handlers.Role
{
    public class AddRoleCommandHandler : IRequestHandler<AddRoleCommand, ResponseModel<string>>
    {
        private readonly RoleManager<AppRole> roleManager;
        private readonly ApiResponseDto response;
        private readonly ILogger<AddRoleCommandHandler> logger;

        public AddRoleCommandHandler(RoleManager<AppRole> roleManager,
            ApiResponseDto response,
            ILogger<AddRoleCommandHandler> logger)
        {
            this.roleManager = roleManager;
            this.response = response;
            this.logger = logger;
        }

        public async Task<ResponseModel<string>> Handle(AddRoleCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.RoleName))
                return response.Process<string>(new BadRequestResponse("Role name is required."));

            var existing = await roleManager.RoleExistsAsync(request.RoleName);
            if (existing)
            {
                logger.LogWarning($"Role {request.RoleName} already exists.");
                return response.Process<string>(new BadRequestResponse($"Role {request.RoleName} already exists."));
            }

            var result = await roleManager.CreateAsync(new AppRole
            {
                Name = request.RoleName.RemoveSpaceAndCapitalize(),
                NormalizedName = request.RoleName.ToUpper()
            });

            if (!result.Succeeded)
            {
                var errorMessages = string.Join(Environment.NewLine, result.Errors);
                logger.LogError($"Error occurred: {errorMessages}");
                return response
                    .Process<string>(new BadRequestResponse($"Operation failed! {result.Errors.FirstOrDefault()?.Description}"));
            }

            return response.Process<string>(new ApiOkResponse<string>($"Role {request.RoleName} successfully added."));
        }
    }
}
