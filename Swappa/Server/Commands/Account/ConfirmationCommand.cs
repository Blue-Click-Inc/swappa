using MediatR;
using Microsoft.Extensions.Primitives;
using Swappa.Shared.DTOs;
using System.Text.Json.Serialization;

namespace Swappa.Server.Commands.Account
{
    public record ConfirmationCommand : AccountConfirmationDto, IRequest<ResponseModel<string>>
    {
        [JsonIgnore]
        public StringValues Origin { get; set; }
    }
}
