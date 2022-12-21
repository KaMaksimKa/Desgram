using System.ComponentModel.DataAnnotations;

namespace Desgram.Api.Models.User
{
    public class TryCreateUserModel
    {

        [Required(ErrorMessage = "Это обязательное поле.")]
        [RegularExpression(@"[A-Za-z0-9._]*",ErrorMessage = "В именах пользователей можно использовать только буквы, цифры, символы подчеркивания и точки.")]
        public string UserName { get; init; } = null!;

        [Required(ErrorMessage = "Это обязательное поле.")]
        [EmailAddress(ErrorMessage = "Введите действительный адрес электронной почты.")]
        public string Email { get; init; } = null!;

        [Required(ErrorMessage = "Это обязательное поле.")]
        [MinLength(6,ErrorMessage = "Минимальная длина пароля 6 символов.")]
        public string Password { get; init; } = null!;

        [Required(ErrorMessage = "Это обязательное поле.")]
        [Compare(nameof(Password),ErrorMessage = "Пароли должны совпадать.")]
        public string RetryPassword { get; init; } = null!;



    }
}
