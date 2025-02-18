﻿using MediatR;
using Swappa.Shared.DTOs;
using Swappa.Shared.DTOs.Interfaces;

namespace Swappa.Server.Commands.Account
{
    public sealed record DeactivateCommand : IBaseIdDto, IRequest<ResponseModel<string>>
    {
        public Guid Id { get; set; }
    }
}
