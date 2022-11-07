using System.ComponentModel.DataAnnotations;

namespace Desgram.Api.Models
{
    public class TokenRequestModel
    {
        [Required]
        public string UserName { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;

    }
}
