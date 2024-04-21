﻿using MediatR;
using Swappa.Data.Contracts;
using Swappa.Entities.Exceptions;
using Swappa.Entities.Responses;
using Swappa.Server.Commands.Account;
using Swappa.Server.Handlers;

namespace Swappa.Server.Handlers.Account
{
    public class LoginHandler : IRequestHandler<LoginCommand, ResponseModel<string>>
    {
        private readonly ILogger<LoginHandler> logger;
        private readonly IRepositoryManager repository;

        public LoginHandler(ILogger<LoginHandler> logger, IRepositoryManager repository)
        {
            this.logger = logger;
            this.repository = repository;
        }

        public async Task<ResponseModel<string>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            await Task.Run(() => Console.WriteLine());
            return new ResponseModel<string>();
        }
    }
}