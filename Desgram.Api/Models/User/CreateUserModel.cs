using System.ComponentModel.DataAnnotations;

namespace Desgram.Api.Models.User
{
    public class CreateUserModel
    {
        [Required] public TryCreateUserModel TryCreateUserModel { get; set; } = null!;

        [Required] public EmailCodeModel EmailCodeModel { get; set; } = null!;

    }
}
