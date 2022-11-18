﻿using System.ComponentModel.DataAnnotations;

namespace Desgram.Api.Models.User
{
    public class CreateUserModel
    {

        [Required]
        [RegularExpression(@"[A-Za-z0-9._]*")]
        public string UserName { get; init; } = null!;

        [Required]
        [EmailAddress]
        public string Email { get; init; } = null!;

        [Required]
        [MinLength(6)]
        public string Password { get; init; } = null!;

        [Required]
        [Compare(nameof(Password))]
        public string RetryPassword { get; init; } = null!;



    }
}
