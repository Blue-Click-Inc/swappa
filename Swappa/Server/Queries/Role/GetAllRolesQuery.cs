using MediatR;
using Swappa.Shared.DTOs;

namespace Swappa.Server.Queries.Role
{
    public class GetAllRolesQuery : BasePageDto, IRequest<ResponseModel<PaginatedListDto<RoleDto>>>
    {
    }
}
