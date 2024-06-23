﻿using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Swappa.Data.Contracts;
using Swappa.Entities.Models;
using Swappa.Entities.Responses;
using Swappa.Server.Queries.User;
using Swappa.Shared.DTOs;
using Swappa.Shared.Extensions;

namespace Swappa.Server.Handlers.User
{
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, ResponseModel<UserDetailsDto>>
    {
        private readonly UserManager<AppUser> userManager;
        private readonly RoleManager<AppRole> roleManager;
        private readonly ApiResponseDto response;
        private readonly IRepositoryManager repository;
        private readonly IMapper mapper;

        public GetUserByIdQueryHandler(UserManager<AppUser> userManager,
            RoleManager<AppRole> roleManager,
            ApiResponseDto response, 
            IRepositoryManager repository,
            IMapper mapper)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.response = response;
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<ResponseModel<UserDetailsDto>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            if(request == null || request.Id.IsEmpty())
            {
                return response.Process<UserDetailsDto>(new BadRequestResponse("Invalid request parameter(s)."));
            }

            var user = await userManager.FindByIdAsync(request.Id.ToString());
            if(user == null)
            {
                return response.Process<UserDetailsDto>(new BadRequestResponse($"No user found with Id: {request.Id}."));
            }

            var userDetails = mapper.Map<UserDetailsDto>(user);
            var userRoles = roleManager.Roles
                .Where(r => user.Roles.Contains(r.Id))
                .ToList();
            if (userRoles.IsNotNullOrEmpty())
            {
                userDetails.Roles = string.Join(",", userRoles);
            }

            var location = await repository.Location.GetByConditionAsync(l => l.EntityId.Equals(user.Id));
            if(location != null)
            {
                userDetails.Location = mapper.Map<LocationToReturnDto>(location);
            }
            return response.Process<UserDetailsDto>(new ApiOkResponse<UserDetailsDto>(userDetails));
        }
    }
}
