using System.ComponentModel.DataAnnotations;

namespace Desgram.Api.Models.User
{
    public class ConfirmEmailModel
    {
        [Required]
        public Guid UnconfirmedEmailId { get; set; }

        [Required] 
        public string ConfirmCode { get; set; } = null!;
    }
}
