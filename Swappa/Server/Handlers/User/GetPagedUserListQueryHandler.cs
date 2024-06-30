using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Swappa.Entities.Models;
using Swappa.Entities.Responses;
using Swappa.Server.Extensions;
using Swappa.Server.Queries.User;
using Swappa.Shared.DTOs;

namespace Swappa.Server.Handlers.User
{
    public class GetPagedUserListQueryHandler : IRequestHandler<GetPagedUserListQuery, ResponseModel<PaginatedListDto<LeanUserDetailsDto>>>
    {
        private readonly UserManager<AppUser> userManager;
        private readonly ApiResponseDto response;
        private readonly IMapper mapper;

        public GetPagedUserListQueryHandler(UserManager<AppUser> userManager,
            ApiResponseDto response,
            IMapper mapper)
        {
            this.userManager = userManager;
            this.response = response;
            this.mapper = mapper;
        }

        public async Task<ResponseModel<PaginatedListDto<LeanUserDetailsDto>>> Handle(GetPagedUserListQuery request, CancellationToken cancellationToken)
        {
            var userQuery = userManager.Users
                .OrderByDescending(u => u.CreatedOn)
                .Search(request.SearchBy);

            var pageList = await Task.Run(() => 
                PagedList<AppUser>.ToPagedList(userQuery, request.PageNumber, request.PageSize));

            var data = mapper.Map<IEnumerable<LeanUserDetailsDto>>(pageList);
            var pagedDataList = PaginatedListDto<LeanUserDetailsDto>.Paginate(data, pageList.MetaData);
            return response.Process<PaginatedListDto<LeanUserDetailsDto>>(new ApiOkResponse<PaginatedListDto<LeanUserDetailsDto>>(pagedDataList));
        }
    }
}
