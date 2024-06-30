using MediatR;
using Microsoft.AspNetCore.Identity;
using Swappa.Entities.Enums;
using Swappa.Entities.Models;
using Swappa.Entities.Responses;
using Swappa.Server.Queries.User;
using Swappa.Shared.DTOs;

namespace Swappa.Server.Handlers.User
{
    public class GetUserDashboardQueryHandler : IRequestHandler<GetUserDashboardQuery, ResponseModel<UserDashboardDataDto>>
    {
        private readonly UserManager<AppUser> userManager;
        private readonly ApiResponseDto response;

        public GetUserDashboardQueryHandler(UserManager<AppUser> userManager,
            ApiResponseDto response)
        {
            this.userManager = userManager;
            this.response = response;
        }

        public async Task<ResponseModel<UserDashboardDataDto>> Handle(GetUserDashboardQuery request, CancellationToken cancellationToken)
        {
            var userQuery = userManager.Users;

            var totalNumbers = await Task.Run(() => userQuery.LongCount());
            var activeUsersCount = await Task.Run(() => userQuery.LongCount(u => u.Status.Equals(Status.Active)));
            var inactiveUsersCount = await Task.Run(() => userQuery.LongCount(u => u.Status.Equals(Status.Inactive)));

            return response.Process<UserDashboardDataDto>(new ApiOkResponse<UserDashboardDataDto>(new UserDashboardDataDto
            {
                TotalCount = totalNumbers,
                ActiveCount = activeUsersCount,
                InactiveCount = inactiveUsersCount
            }));
        }
    }
}
