using System.ComponentModel.DataAnnotations;

namespace Desgram.Api.Models.User
{
    public class ChangePasswordModel
    {
        [Required(ErrorMessage = "Это обязательное поле.")]
        public string OldPassword { get; set; } = null!;

        [Required(ErrorMessage = "Это обязательное поле.")]
        [MinLength(6, ErrorMessage = "Минимальная длина пароля 6 символов.")]
        public string NewPassword { get; init; } = null!;

        [Required(ErrorMessage = "Это обязательное поле.")]
        [Compare(nameof(NewPassword), ErrorMessage = "Пароли должны совпадать.")]
        public string RetryNewPassword { get; init; } = null!;
    }
}
