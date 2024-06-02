using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Swappa.Entities.Models;
using Swappa.Entities.Responses;
using Swappa.Server.Commands.Role;
using Swappa.Shared.Extensions;
using Swappa.Shared.DTOs;

namespace Swappa.Server.Handlers.Role
{
    public class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleCommand, ResponseModel<string>>
    {
        private readonly RoleManager<AppRole> roleManager;
        private readonly IMapper mapper;
        private readonly ApiResponseDto response;

        public UpdateRoleCommandHandler(RoleManager<AppRole> roleManager, 
            IMapper mapper, 
            ApiResponseDto response)
        {
            this.roleManager = roleManager;
            this.mapper = mapper;
            this.response = response;
        }

        public async Task<ResponseModel<string>> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
        {
            if (request == null || request.Id.IsEmpty())
                return response.Process<string>(new BadRequestResponse($"Invalid request parameter(s)."));

            var role = await roleManager.FindByIdAsync(request.Id.ToString());
            if (role == null)
                return response.Process<string>(new BadRequestResponse($"No role record found with Id: {request.Id}"));

            mapper.Map(request, role);
            await roleManager.UpdateAsync(role);

            return response.Process<string>(new ApiOkResponse<string>("Role successfully updated."));
        }
    }
}
