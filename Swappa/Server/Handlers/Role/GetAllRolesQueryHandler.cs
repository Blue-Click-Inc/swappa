using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Swappa.Entities.Models;
using Swappa.Server.Queries.Role;
using Swappa.Shared.DTOs;

namespace Swappa.Server.Handlers.Role
{
    public class GetAllRolesQueryHandler : IRequestHandler<GetAllRolesQuery, PaginatedListDto<RoleDto>>
    {
        private readonly RoleManager<AppRole> roleManager;
        private readonly UserManager<AppUser> userManager;
        private readonly IMapper mapper;

        public GetAllRolesQueryHandler(RoleManager<AppRole> roleManager,
            UserManager<AppUser> userManager,
            IMapper mapper)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.mapper = mapper;
        }

        public async Task<PaginatedListDto<RoleDto>> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
        {
            var roles = await Task.Run(() => roleManager.Roles.AsQueryable()
                .OrderBy(r => r.Name)
                .ToList());

            var dto = mapper.Map<IEnumerable<RoleDto>>(roles);
            foreach (var item in dto)
            {
                item.NumberOfUser = (await userManager.GetUsersInRoleAsync(item.RoleName)).LongCount();
            }
            var pagedList = PagedList<RoleDto>.ToPagedList(dto, request.PageNumber, request.PageSize);

            return PaginatedListDto<RoleDto>.Paginate(pagedList, pagedList.MetaData);
        }
    }
}
