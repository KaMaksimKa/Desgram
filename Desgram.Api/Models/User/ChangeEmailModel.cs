﻿using System.ComponentModel.DataAnnotations;

namespace Desgram.Api.Models.User
{
    public class ChangeEmailModel
    {
        [Required] 
        [EmailAddress] 
        public string NewEmail { get; set; } = null!;

        [Required] public EmailCodeModel EmailCodeModel { get; set; } = null!;
    }
}
