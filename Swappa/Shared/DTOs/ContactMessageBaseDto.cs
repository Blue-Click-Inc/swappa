﻿using System.ComponentModel.DataAnnotations;

namespace Swappa.Shared.DTOs
{
    public abstract class ContactMessageBaseDto
    {
        [Required(ErrorMessage = "Name field is required")]
        public string FullName { get; set; } = string.Empty;
        [Required(ErrorMessage = "Email field is required"), EmailAddress]
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        [Required(ErrorMessage = "Message field is required.")]
        public string Message { get; set; } = string.Empty;
    }
}
