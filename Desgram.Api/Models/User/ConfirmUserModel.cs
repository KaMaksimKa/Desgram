using System.ComponentModel.DataAnnotations;

namespace Desgram.Api.Models.User
{
    public class ConfirmUserModel
    {
        [Required]
        public Guid UnconfirmedUserId { get; set; }

        [Required] 
        public string ConfirmCode { get; set; } = null!;
    }
}
