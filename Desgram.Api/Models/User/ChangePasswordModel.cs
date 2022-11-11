using System.ComponentModel.DataAnnotations;

namespace Desgram.Api.Models.User
{
    public class ChangePasswordModel
    {
        [Required]
        public string OldPassword { get; set; } = null!;

        [Required]
        [MinLength(6)]
        public string NewPassword { get; init; } = null!;

        [Required]
        [Compare(nameof(NewPassword))]
        public string RetryNewPassword { get; init; } = null!;
    }
}
