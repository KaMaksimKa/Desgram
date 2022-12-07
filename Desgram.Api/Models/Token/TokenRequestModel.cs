using System.ComponentModel.DataAnnotations;

namespace Desgram.Api.Models.Token
{
    public class TokenRequestModel
    {
        [Required]
        public string Login { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;

    }
}
