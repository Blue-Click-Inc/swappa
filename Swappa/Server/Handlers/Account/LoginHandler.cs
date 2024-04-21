using MediatR;
using Microsoft.AspNetCore.Identity;
using Swappa.Data.Contracts;
using Swappa.Entities.Models;
using Swappa.Entities.Responses;
using Swappa.Server.Commands.Account;
using Swappa.Shared.DTOs;

namespace Swappa.Server.Handlers.Account
{
    public class LoginHandler : IRequestHandler<LoginCommand, ResponseModel<string>>
    {
        private readonly ILogger<LoginHandler> logger;
        private readonly IRepositoryManager repository;
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly ApiResponseDto response;

        public LoginHandler(
            ILogger<LoginHandler> logger, 
            IRepositoryManager repository,
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            ApiResponseDto response)
        {
            this.logger = logger;
            this.repository = repository;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.response = response;
        }

        public async Task<ResponseModel<string>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            await Task.Run(() => Console.WriteLine());
            return response.Process<string>(new ApiOkResponse<string>("Login successful"));
        }
    }
}