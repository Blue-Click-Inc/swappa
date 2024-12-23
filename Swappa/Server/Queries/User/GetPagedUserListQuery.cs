﻿using MediatR;
using Swappa.Shared.DTOs;

namespace Swappa.Server.Queries.User
{
    public class GetPagedUserListQuery : SearchDto, IRequest<ResponseModel<PaginatedListDto<LeanUserDetailsDto>>>
    {
    }
}
