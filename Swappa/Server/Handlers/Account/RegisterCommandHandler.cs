using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Swappa.Data.Contracts;
using Swappa.Data.Services.Interfaces;
using Swappa.Entities.Enums;
using Swappa.Entities.Models;
using Swappa.Entities.Responses;
using Swappa.Server.Commands.Account;
using Swappa.Shared.Extensions;
using Swappa.Server.Validations.Account;
using Swappa.Shared.DTOs;

namespace Swappa.Server.Handlers.Account
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, ResponseModel<string>>
    {
        private readonly UserManager<AppUser> userManager;
        private readonly ApiResponseDto response;
        private readonly IMapper mapper;
        private readonly INotify notify;

        public RegisterCommandHandler(UserManager<AppUser> userManager,
            ApiResponseDto response,
            IMapper mapper,
            INotify notify)
        {
            this.userManager = userManager;
            this.response = response;
            this.mapper = mapper;
            this.notify = notify;
        }

        public async Task<ResponseModel<string>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            if (!request.MatchPassword)
                return response.Process<string>(new BadRequestResponse("Password and Confirm Password must match."));

            var validator = new RegisterValidator();
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
                return response.Process<string>(new BadRequestResponse(validationResult.Errors.FirstOrDefault()?.ErrorMessage!));

            var user = await userManager.FindByEmailAsync(request.Email);
            if (user != null)
                return response
                    .Process<string>(new BadRequestResponse($"A user already exists with this email: {request.Email}."));            

            user = mapper.Map<AppUser>(request);
            user.UserName = request.Email;
            user.Name = user.Name.Capitalize();

            var result = await userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
                return response.Process<string>(new BadRequestResponse($"Registration failed. {result.Errors.FirstOrDefault()?.Description}"));

            var roleResult = await AssignToRoleAsync(user.Id.ToString(), request.Role.ToString());
            if (!roleResult.IsSuccessful)
                return roleResult;

            return await notify.SendAccountEmailAsync(user, request.Origin, TokenType.AccountConfirmation);
        }

        private async Task<ResponseModel<string>> AssignToRoleAsync(string id, string role)
        {
            var user = await userManager.FindByIdAsync(id);
            if(user == null)
            {
                return response.Process<string>(new BadRequestResponse($"Registration failed. Couldn't find user."));
            }
            var roleResult = await userManager.AddToRoleAsync(user, role);
            if (!roleResult.Succeeded)
            {
                await userManager.DeleteAsync(user);
                return response.Process<string>(new BadRequestResponse($"Registration failed. {roleResult.Errors.FirstOrDefault()?.Description}"));
            }

            return response.Process<string>(new ApiOkResponse<string>($"{role} role successfully assigned to user"));
        }
    }
}
