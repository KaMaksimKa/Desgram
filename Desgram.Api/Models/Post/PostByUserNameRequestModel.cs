using System.ComponentModel.DataAnnotations;

namespace Desgram.Api.Models.Post
{
    public class PostByUserNameRequestModel
    {
        [Required]
        public int Skip { get; set; }

        [Required]
        public int Take { get; set; }

        [Required]
        public string UserName { get; set; } = null!;
    }
}
