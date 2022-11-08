using System.ComponentModel.DataAnnotations;

namespace Desgram.Api.Models.Token
{
    public class RefreshTokenRequestModel
    {
        [Required]
        public string RefreshToken { get; set; } = null!;

    }
}
