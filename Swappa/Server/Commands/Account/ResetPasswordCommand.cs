using MediatR;
using Microsoft.Extensions.Primitives;
using Swappa.Shared.DTOs;
using System.Text.Json.Serialization;

namespace Swappa.Server.Commands.Account
{
    public sealed record ResetPasswordCommand : EmailDto, IRequest<ResponseModel<string>>
    {
        [JsonIgnore]
        public StringValues Origin { get; set; }
    }
}
