﻿using Microsoft.Extensions.Primitives;
using Swappa.Entities.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Swappa.Shared.DTOs
{
    public record RegisterDto : BaseAccountDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        [Compare("Password", ErrorMessage = "Password and Compare Password fields must match")]
        public string ConfirmPassword { get; set; } = string.Empty;
        public Gender Gender { get; set; } = Gender.NotSpecified;
        public SystemRole Role { get; set; } = SystemRole.User;
        [JsonIgnore]
        public StringValues Origin { get; set; }
        [JsonIgnore]
        public bool MatchPassword => !string.IsNullOrWhiteSpace(Password) && 
            !string.IsNullOrWhiteSpace(ConfirmPassword) && Password.Equals(ConfirmPassword);
    }
}
