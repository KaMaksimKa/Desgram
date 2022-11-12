using System.ComponentModel.DataAnnotations;

namespace Desgram.Api.Models.Post
{
    public class PostByHashtagRequestModel
    {
        [Required]
        public int Skip { get; set; }

        [Required]
        public int Take { get; set; }

        [Required] 
        public string Hashtag { get; set; } = null!;
    }
}
