using System.ComponentModel.DataAnnotations;

namespace Desgram.Api.Models.Post
{
    public class PostRequestModel
    {
        [Required]
        public int Skip { get; set; }

        [Required]
        public int Take { get; set; }
    }
}
