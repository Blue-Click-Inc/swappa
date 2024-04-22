using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Primitives;
using Swappa.Data.Contracts;
using Swappa.Data.Services.Interfaces;
using Swappa.Entities.Enums;
using Swappa.Entities.Models;
using Swappa.Entities.Responses;
using Swappa.Server.Commands.Account;
using Swappa.Server.Extensions;
using Swappa.Shared.DTOs;

namespace Swappa.Server.Handlers.Account
{
    public class RegisterHandler : IRequestHandler<RegisterCommand, ResponseModel<string>>
    {
        private readonly ILogger<RegisterHandler> logger;
        private readonly IRepositoryManager repository;
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly ApiResponseDto response;
        private readonly IMapper mapper;
        private readonly IConfiguration configuration;
        private readonly INotify notify;

        public RegisterHandler(ILogger<RegisterHandler> logger, 
            IRepositoryManager repository,
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            ApiResponseDto response,
            IMapper mapper,
            IConfiguration configuration,
            INotify notify)
        {
            this.logger = logger;
            this.repository = repository;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.response = response;
            this.mapper = mapper;
            this.configuration = configuration;
            this.notify = notify;
        }

        public async Task<ResponseModel<string>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            //TODO: Do validation
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

            var roleResult = await AssignToRoleAsync(user.Email, request.Role.ToString());
            if (!roleResult.IsSuccessful)
                return roleResult;

            var messageResult = await SendConfirmationEmail(user, request.Origin);
            return messageResult;
        }

        private async Task<ResponseModel<string>> AssignToRoleAsync(string email, string role)
        {
            var user = await userManager.FindByEmailAsync(email);
            var roleResult = await userManager.AddToRoleAsync(user, role);
            if (!roleResult.Succeeded)
            {
                await userManager.DeleteAsync(user);
                return response.Process<string>(new BadRequestResponse($"Registration failed. {roleResult.Errors.FirstOrDefault()?.Description}"));
            }

            return response.Process<string>(new ApiOkResponse<string>($"{role} role successfully assigned to user"));
        }

        private async Task<ResponseModel<string>> SendConfirmationEmail(AppUser user, StringValues origin)
        {
            var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
            var url = origin.BuildUrl(token, user.Id);
            var message = Statics.GetAccountConfirmationTemplate(url, user.Name);

            var success = await notify.SendAsync(user.Email, message, "Account Confirmation");
            if (success.GetValueOrDefault())
            {
                var tokenToAdd = new Token
                {
                    UserId = user.Id,
                    Type = TokenType.AccountConfirmation,
                    Value = token
                };
                await repository.Token.AddAsync(tokenToAdd);
                return response.Process<string>(new ApiOkResponse<string>("Registration successful. Please check your email to confirm your account"));
            }
            else
            {
                var userToDelete = await userManager.FindByIdAsync(user.Id.ToString());
                if (userToDelete != null)
                {
                    await userManager.DeleteAsync(userToDelete);
                }
                return response.Process<string>(new BadRequestResponse("Registration failed! Please try again later."));
            }
        }
    }
}
