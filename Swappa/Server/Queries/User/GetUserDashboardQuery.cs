using MediatR;
using Swappa.Shared.DTOs;

namespace Swappa.Server.Queries.User
{
    public class GetUserDashboardQuery : IRequest<ResponseModel<UserDashboardDataDto>>
    {
    }
}
