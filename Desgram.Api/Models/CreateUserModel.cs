using System.ComponentModel.DataAnnotations;

namespace Desgram.Api.Models
{
    public class CreateUserModel
    {

        [Required]
        public string Name { get; init; }

        [Required]
        public string Email { get; init; }

        [Required]
        [MinLength(6)]
        public string Password { get; init; }

        [Required]
        [Compare(nameof(Password))]
        public string RetryPassword { get; init; }

        [Required]
        public DateTimeOffset BirthDate { get; init; }


        public CreateUserModel(string name, string email, string password, string retryPassword, DateTimeOffset birthDate)
        {
            Name = name;
            Email = email;
            Password = password;
            RetryPassword = retryPassword;
            BirthDate = birthDate;
        }

    }
}
