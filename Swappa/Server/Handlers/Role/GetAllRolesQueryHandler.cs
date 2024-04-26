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
        private readonly IMapper mapper;

        public GetAllRolesQueryHandler(RoleManager<AppRole> roleManager, IMapper mapper)
        {
            this.roleManager = roleManager;
            this.mapper = mapper;
        }

        public async Task<PaginatedListDto<RoleDto>> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
        {
            var roles = await Task.Run(() => roleManager.Roles.AsQueryable()
                .OrderBy(r => r.Name)
                .ToList());

            var dto = mapper.Map<IEnumerable<RoleDto>>(roles);
            var pagedList = PagedList<RoleDto>.ToPagedList(dto, request.PageNumber, request.PageSize);

            return PaginatedListDto<RoleDto>.Paginate(pagedList, pagedList.MetaData);
        }
    }
}
