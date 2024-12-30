﻿using System.ComponentModel.DataAnnotations;

namespace Swappa.Shared.DTOs
{
    public class ResponseMessageDto
    {
        [Required(ErrorMessage = "subject field is required.")]
        public string Subject { get; set; } = string.Empty;
        [Required(ErrorMessage = "Message field is required.")]
        public string Body { get; set; } = string.Empty;
    }
}