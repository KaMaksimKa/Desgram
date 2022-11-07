using System.ComponentModel.DataAnnotations;

namespace Desgram.Api.Models
{
    public class RefreshTokenRequestModel
    {
        [Required]
        public string RefreshToken { get; set; } = null!;

    }
}
