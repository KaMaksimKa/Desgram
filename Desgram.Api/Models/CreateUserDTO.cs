using System.ComponentModel.DataAnnotations;

namespace Desgram.Api.Models
{
    public class CreateUserDTO
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

    }
}
