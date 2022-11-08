using System.ComponentModel.DataAnnotations;

namespace Desgram.Api.Models.Token
{
    public class TokenRequestModel
    {
        [Required]
        public string UserName { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;

    }
}
