using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Swappa.Entities.Models;
using Swappa.Entities.Responses;
using Swappa.Server.Queries.Role;
using Swappa.Shared.DTOs;

namespace Swappa.Server.Handlers.Role
{
    public class GetAllRolesQueryHandler : IRequestHandler<GetAllRolesQuery, ResponseModel<PaginatedListDto<RoleDto>>>
    {
        private readonly RoleManager<AppRole> roleManager;
        private readonly UserManager<AppUser> userManager;
        private readonly IMapper mapper;
        private readonly ApiResponseDto response;

        public GetAllRolesQueryHandler(RoleManager<AppRole> roleManager,
            UserManager<AppUser> userManager,
            IMapper mapper, ApiResponseDto response)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.mapper = mapper;
            this.response = response;
        }

        public async Task<ResponseModel<PaginatedListDto<RoleDto>>> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
        {
            var roles = await Task.Run(() => roleManager.Roles.AsQueryable()
                .OrderBy(r => r.Name)
                .ToList());

            var dto = mapper.Map<IEnumerable<RoleDto>>(roles);
            foreach (var item in dto)
            {
                item.NumberOfUser = (await userManager.GetUsersInRoleAsync(item.RoleName)).Count;
            }
            var pagedList = PagedList<RoleDto>.ToPagedList(dto, request.PageNumber, request.PageSize);
            var result = PaginatedListDto<RoleDto>.Paginate(pagedList, pagedList.MetaData);
            return response.Process<PaginatedListDto<RoleDto>>(new ApiOkResponse<PaginatedListDto<RoleDto>>(result));
        }
    }
}
