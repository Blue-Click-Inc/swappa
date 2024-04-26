using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Swappa.Entities.Models;
using Swappa.Entities.Responses;
using Swappa.Server.Extensions;
using Swappa.Server.Queries.Role;
using Swappa.Shared.DTOs;

namespace Swappa.Server.Handlers.Role
{
    public class GetRoleByIdQueryHandler : IRequestHandler<GetRolebyIdQuery, ResponseModel<RoleDto>>
    {
        private readonly RoleManager<AppRole> roleManager;
        private readonly ApiResponseDto response;
        private readonly IMapper mapper;

        public GetRoleByIdQueryHandler(RoleManager<AppRole> roleManager, 
            ApiResponseDto response,
            IMapper mapper)
        {
            this.roleManager = roleManager;
            this.response = response;
            this.mapper = mapper;
        }

        public async Task<ResponseModel<RoleDto>> Handle(GetRolebyIdQuery request, CancellationToken cancellationToken)
        {
            if (request == null || request.Id.IsEmpty())
                return response.Process<RoleDto>(new BadRequestResponse($"Invalid request parameter(s)."));

            var role = await roleManager.FindByIdAsync(request.Id.ToString());
            if(role == null)
                return response.Process<RoleDto>(new BadRequestResponse($"No role record found with Id: {request.Id}"));

            return response.Process<RoleDto>(new ApiOkResponse<RoleDto>(mapper.Map<RoleDto>(role)));
        }
    }
}
